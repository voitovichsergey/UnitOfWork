using BookStore.Domain.Core;
using BookStore.Domain.Interfaces;
using BookStore.Infrastructure.Data.DataModel;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Data
{
    public sealed class BookRepository(DbContext context) : IBookRepository
    {
        private readonly BookStoreContext _db = (BookStoreContext)context;

        public async Task<Book> GetAsync(long bookId)
        {
            return await _db.Books.SingleAsync(x => x.Id == bookId);
        }

        public async Task AppendAsync(long bookId, int count)
        {
            var book = await _db.Books.SingleOrDefaultAsync(x => x.Id == bookId)
                ?? throw new NullReferenceException("Book not found");
            book.Count += count;
        }

        public async Task<Book> CreateAsync(Book book)
        {
            var newBook = new Book
            {
                Title = book.Title,
                Author = book.Author,
            };
            await _db.Books.AddAsync(newBook);

            return newBook;
        }

        public async Task<Book[]> GetAsync()
        {
            return await _db.Books.AsNoTracking().ToArrayAsync();
        }
    }
}
