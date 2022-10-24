using LibraryAPI.Enums;
using LibraryAPI.Exceptions;
using LibraryAPI.Interfaces;
using LibraryAPI.Models;
using LibraryAPI.Models.Dto;
using LibraryAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Services
{
    public class LibraryBooksService : ILibraryBooksService
    {

        private readonly ILogger<LibraryBooksService> _logger;
        private readonly AppDbContext _context;
        private readonly IAuditService _auditService;

        public LibraryBooksService(
            ILogger<LibraryBooksService> logger,
            AppDbContext context,
            IAuditService auditService)
        {
            _logger = logger;
            _context = context;
            _auditService = auditService;
        }

        public object Add(BookDto dto)
        {
            var book = new Book()
            {
                 AuthorFirstName = dto.AuthorFirstName,
                 AuthorLastName = dto.AuthorLastName,
                 Description = dto.Description,
                 Title = dto.Title,
                 BookInLibrary = new BookInLibrary()
                 {
                      NumOfAvailable = dto.TotalBooks,
                      NumOfRented = 0,
                      TotalBooks = dto.TotalBooks,
                 }
            };

            _context.Books.Add(book);
            _context.SaveChanges();

            _auditService.AuditDbTable(DbTables.BOOKS, book.Id.ToString(), DbOperations.INSERT, "");

            return new
            {
                book.Id,
                book.Author,
                book.AuthorFirstName,
                book.AuthorLastName,
                book.Title,
                book.Description,
                book.BookInLibrary.NumOfRented,
                book.BookInLibrary.NumOfAvailable,
                book.BookInLibrary.TotalBooks,
            };
        }

        public object Get(int? id = null)
        {
            var query = _context.Books
                .AsNoTracking()
                .Include(x => x.BookInLibrary)
                .Select(x => new {
                    x.Id,
                    x.AuthorFirstName,
                    x.AuthorLastName,
                    x.Author,
                    x.Title,
                    x.Description,
                    x.BookInLibrary.NumOfRented,
                    x.BookInLibrary.NumOfAvailable,
                    x.BookInLibrary.TotalBooks,
                });

            if (id != null) {
                query = query.Where(x => x.Id == id);
            }

            var result = query.ToList();

            return result;
        }

        public int Remove(int id)
        {
            var entity = _context.Books
                .Include(x => x.BookInLibrary)
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == id);

            if (entity is null)
            {
                throw new NotFoundException($"Remove => NOT FOUND book (id: {id})");
            }

            var numOfRented = entity.BookInLibrary.NumOfRented;

            if (numOfRented > 0)
            {
                throw new BadHttpRequestException($"Remove => some books (id: {id}) are rented");
            }

            _context.Books.Remove(entity);
            _context.SaveChanges();

            _auditService.AuditDbTable(DbTables.BOOKS, entity.Id.ToString(), DbOperations.DELETE, "");

            return 200;
        }

        public object Update(int id, BookDto dto)
        {
            var entity = _context.Books
                .FirstOrDefault(x => x.Id == id);

            if (entity is null)
            {
                throw new NotFoundException($"Update => NOT FOUND book (id: {id})");
            }

            entity.AuthorFirstName = dto.AuthorFirstName;
            entity.AuthorLastName = dto.AuthorLastName;
            entity.Title = dto.Title;
            entity.Description = dto.Description;
         
            _context.SaveChanges();

            _auditService.AuditDbTable(DbTables.BOOKS, entity.Id.ToString(), DbOperations.UPDATE, "AuthorFirstName, AuthorLastName, Title, Description");

            return new
            {
                entity.Id,
                entity.Author,
                entity.AuthorFirstName,
                entity.AuthorLastName,
                entity.Title,
                entity.Description,
                entity.BookInLibrary.NumOfRented,
                entity.BookInLibrary.NumOfAvailable,
                entity.BookInLibrary.TotalBooks,
            }; ;
        }

        public object UpdateTotalQuantity(int id, int quantity)
        {
            var entity = _context.Books
                  .Include(x => x.BookInLibrary)
                  .FirstOrDefault(x => x.Id == id);

            if (entity is null)
            {
                throw new NotFoundException($"UpdateTotalQuantity => NOT FOUND book (id: {id})");
            }

            var totalBooks = entity.BookInLibrary.TotalBooks;
            var numOfAvailable = entity.BookInLibrary.NumOfAvailable;

            if (totalBooks == quantity)
            {
                return entity;
            }

            if (totalBooks < quantity)
            {
                entity.BookInLibrary.TotalBooks = quantity;
                entity.BookInLibrary.NumOfAvailable += (quantity - totalBooks);
            } else
            {
                if (totalBooks > quantity && numOfAvailable >= quantity)
                {
                    entity.BookInLibrary.TotalBooks = quantity;
                    entity.BookInLibrary.NumOfAvailable -= quantity;
                } else
                {
                    throw new Exception($"UpdateTotalQuantity => book (id: {id}) not enought availables books (numOfAvailable < quantity: {numOfAvailable} < {quantity} == FALSE)");
                }
            }

            _context.SaveChanges();

            _auditService.AuditDbTable(DbTables.BOOKS, entity.Id.ToString(), DbOperations.UPDATE, "TotalBooks");

            return new
            {
                entity.Id,
                entity.Author,
                entity.AuthorFirstName,
                entity.AuthorLastName,
                entity.Title,
                entity.Description,
                entity.BookInLibrary.NumOfRented,
                entity.BookInLibrary.NumOfAvailable,
                entity.BookInLibrary.TotalBooks,
            }; ;
        }
    }
}
