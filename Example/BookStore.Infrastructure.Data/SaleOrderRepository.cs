using BookStore.Domain.Core;
using BookStore.Domain.Interfaces;
using BookStore.Infrastructure.Data.DataModel;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Data
{
    public sealed class SaleOrderRepository(DbContext context) : ISaleOrderRepository
    {
        private readonly BookStoreContext _db = (BookStoreContext)context;

        public async Task SellAsync(long bookId)
        {
            await _db.AddAsync(new SaleOrder
            {
                Date = DateTime.Now,
                BookId = bookId,
            });
        }
    }
}
