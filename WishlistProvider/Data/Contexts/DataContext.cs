
using Microsoft.EntityFrameworkCore;
using WishlistProvider.Data.Entities;

namespace WishlistProvider.Data.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<WishlistEntity> Wishlists { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WishlistEntity>().HasKey(x => new { x.Email, x.ProductId });
    }
}
