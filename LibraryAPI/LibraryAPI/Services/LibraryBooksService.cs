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

        public LibraryBooksService(
            ILogger<LibraryBooksService> logger,
            AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public object Add(BookDto dto)
        {
            var book = new Book()
            {
                 AuthorFirstName = dto.AuthorFirstName,
                 AuthorLastName = dto.AuthorLastName,
                 Description = dto.Description,
                 Title = dto.Title
            };

            _context.Books.Add(book);
            _context.SaveChanges();

            return book;
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
                 .AsNoTracking()
                 .FirstOrDefault(x => x.Id == id);

            if (entity is null)
            {
                throw new NotFoundException($"Remove => NOT FOUND book (id: {id})");
            }

            _context.Books.Remove(entity);
            _context.SaveChanges();

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

            return entity;
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

            return entity;
        }
    }
}
