using BookStore.Domain.Core;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Data.DataModel
{
    public sealed class BookStoreContext : DbContext
    {
        public DbSet<Book> Books => Set<Book>();
        public DbSet<SaleOrder> Orders => Set<SaleOrder>();

        public BookStoreContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=bookstrore.db");
        }
    }
}
