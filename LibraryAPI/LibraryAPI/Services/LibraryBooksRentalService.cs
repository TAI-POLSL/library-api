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

        public LibraryBooksRentalService(
            ILogger<LibraryBooksRentalService> logger,
            AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public object Add(BookReservationDto dto)
        {
            string user = _context.Users
                .AsNoTracking()
                .Where(x => x.Id == dto.UserId)
                .Select(x => x.Username)
                .FirstOrDefault("");

            if (user == "")
            {
                throw new NotFoundException($"Add => NOT FOUND user (id: {dto.UserId})");
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
                StartDate = DateTime.UtcNow,
                EndDate = dto.EndDate
            };

            booksInLibary.NumOfRented += 1;
            booksInLibary.NumOfAvailable -= 1;

            _context.UsersBooksRented.Add(entity);
            _context.SaveChanges();

            return entity;
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

            var fetch = query.Select(x => new
            {
                x.Id,
                x.Book.AuthorFirstName,
                x.Book.AuthorLastName,
                x.Book.Author,
                x.Book.Title,
                x.StartDate,
                x.EndDate,
                user = new {
                    id = x.User.Id,
                    username = x.User.Username,
                    firstName = x.User.Person.FirstName,
                    lastName = x.User.Person.LastName
                }
            });

            var result = fetch.ToList();

            return result;
        }

        public object Cancel(int id)
        {
            var entity = _context.UsersBooksRented
                .FirstOrDefault(x => x.Id == id);

            if (entity == null)
            {
                throw new NotFoundException($"Cancel => book rented NOT FOUND (id: {id})");
            }

            entity.EndDate = DateTime.UtcNow;

            _context.SaveChanges();

            return entity;
        }

        public object End(int id)
        {
            var entity = _context.UsersBooksRented
                .FirstOrDefault(x => x.Id == id);

            if (entity == null)
            {
                throw new NotFoundException($"End => book rented NOT FOUND (id: {id})");
            }

            entity.EndDate = DateTime.UtcNow;

            _context.SaveChanges();

            return entity;
        }
    }
}
