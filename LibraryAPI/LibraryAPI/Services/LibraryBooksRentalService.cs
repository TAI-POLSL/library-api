using LibraryAPI.Enums;
using LibraryAPI.Exceptions;
using LibraryAPI.Interfaces;
using LibraryAPI.Models;
using LibraryAPI.Models.Dto;
using LibraryAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Services
{
    public class LibraryBooksRentalService : ILibraryBooksRentalService
    {
        private readonly ILogger<LibraryBooksRentalService> _logger;
        private readonly AppDbContext _context;
        private readonly IAuditService _auditService;
        private readonly IHeaderContextService _headerContextService;

        public LibraryBooksRentalService(
            ILogger<LibraryBooksRentalService> logger,
            AppDbContext context,
            IAuditService auditService,
            IHeaderContextService headerContextService)
        {
            _logger = logger;
            _context = context;
            _auditService = auditService;
            _headerContextService = headerContextService;
        }

        public object Add(BookRentByUserDto dto)
        {
            var user = _context.Users
                .AsNoTracking()
                .Where(x => x.Id == dto.UserId)
                .FirstOrDefault();

            if (user is null)
            {
                throw new NotFoundException($"Add => NOT FOUND user (id: {dto.UserId})");
            }

            // Only clients can rents books
            if (user.Role != UserRoles.CLIENT)
            {
                throw new BadHttpRequestException($"Add => Not a client (id: {dto.UserId})");
            }

            var booksInLibary = _context.BooksInLibrary
                .Where(x => x.BookId == dto.BookId)
                .FirstOrDefault();

            if (booksInLibary == null)
            {
                throw new NotFoundException($"Add => NOT FOUND book (id: {dto.BookId})");
            }
 
            var numOfAvaliable = booksInLibary.NumOfAvailable;

            if (numOfAvaliable == 0)
            {
                throw new InvalidOperationException($"Add => Not enought available books");
            } else if (numOfAvaliable < 0)
            {
                throw new Exception($"Add => System corrupt");
            }

            var entity = new UserBookRented
            {
                UserId = dto.UserId,
                BookId = dto.BookId,
                Status = BookRentStatus.OPEN,
                StartDate = DateTime.UtcNow,
                EndDate = dto.EndDate
            };

            booksInLibary.NumOfRented += 1;
            booksInLibary.NumOfAvailable -= 1;

            _context.UsersBooksRented.Add(entity);
            _context.SaveChanges();

            _auditService.AuditDbTable(DbTables.USERS_BOOKS_RENTED, entity.Id.ToString(), DbOperations.INSERT, "");

            return new
            {
                entity.Id,
                entity.BookId,
                entity.UserId,
                entity.Status,
                entity.StartDate,
                entity.EndDate,
            };
        }

        public object Get(int? id = null, Guid? userId = null)
        {
            IQueryable<UserBookRented> query = _context.UsersBooksRented
                .AsNoTracking()
                .Include(x => x.Book)
                .Include(x => x.User)
                .Include(x => x.User.Person);

            if (id != null)
            {
                query = query.Where(x => x.Id == id);
            }

            if (userId != null)
            {
                query = query.Where(x => x.UserId == userId);
            }

            // CLIENT can get only own rentals
            if (_headerContextService.GetUserRole() == UserRoles.CLIENT)
            {
                query = query.Where(x => x.UserId == _headerContextService.GetUserId());

                var fetchOwn = query.Select(x => new
                {
                    x.Id,
                    x.Book.AuthorFirstName,
                    x.Book.AuthorLastName,
                    x.Book.Author,
                    x.Book.Title,
                    x.Status,
                    x.StartDate,
                    x.EndDate,
                });

                return fetchOwn.ToList();
            }

            var fetch = query.Select(x => new
            {
                x.Id,
                x.Book.AuthorFirstName,
                x.Book.AuthorLastName,
                x.Book.Author,
                x.Book.Title,
                x.Status,
                x.StartDate,
                x.EndDate,
                user = new {
                    id = x.User.Id,
                    username = x.User.Username,
                    firstName = x.User.Person.FirstName,
                    lastName = x.User.Person.LastName
                }
            });

            return fetch.ToList();
        }

        public object Cancel(int id)
        {
            var entity = _context.UsersBooksRented
                .FirstOrDefault(x => x.Id == id);

            if (entity == null)
            {
                throw new NotFoundException($"Cancel => book rented NOT FOUND (id: {id})");
            }

            if (entity.Status != BookRentStatus.OPEN)
            {
                throw new NotFoundException($"End => can not change history status");
            }

            entity.EndDate = DateTime.UtcNow;
            entity.Status = BookRentStatus.CANCEL;

            var bookInLibrary = _context.BooksInLibrary
                .Where(x => x.BookId == entity.BookId)
                .FirstOrDefault();

            if (bookInLibrary == null)
            {
                throw new NotFoundException($"Cancel => system corrupt");
            }

            bookInLibrary.NumOfAvailable += 1;
            bookInLibrary.NumOfRented -= 1;

            _context.SaveChanges();

            _auditService.AuditDbTable(DbTables.USERS_BOOKS_RENTED, entity.Id.ToString(), DbOperations.UPDATE, "Cancel");

            return 200;
        }

        public object End(int id)
        {
            var entity = _context.UsersBooksRented
                .FirstOrDefault(x => x.Id == id);

            if (entity == null)
            {
                throw new NotFoundException($"End => book rented NOT FOUND (id: {id})");
            }

            if (entity.Status != BookRentStatus.OPEN)
            {
                throw new NotFoundException($"End => can not change history status");
            }

            entity.EndDate = DateTime.UtcNow;
            entity.Status = BookRentStatus.END;

            var bookInLibrary = _context.BooksInLibrary
               .Where(x => x.BookId == entity.BookId)
               .FirstOrDefault();

            if (bookInLibrary == null)
            {
                throw new NotFoundException($"Cancel => system corrupt");
            }

            bookInLibrary.NumOfAvailable += 1;
            bookInLibrary.NumOfRented -= 1;

            _context.SaveChanges();

            _auditService.AuditDbTable(DbTables.USERS_BOOKS_RENTED, entity.Id.ToString(), DbOperations.UPDATE, "End");

            return 200;
        }
    }
}
