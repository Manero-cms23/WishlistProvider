using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using WishlistProvider.Data.Contexts;
using WishlistProvider.Services;

namespace Wishlist_unittest;

public class UnitTest1
{
    private readonly DataContext _context;
    private readonly AddToWishlistService _addToWishlistService;
    private readonly Mock<ILogger<AddToWishlistService>> _mockLogger;

    public UnitTest1()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _mockLogger = new Mock<ILogger<AddToWishlistService>>();
        _context = new DataContext(options);
        _addToWishlistService = new AddToWishlistService(_mockLogger.Object, _context);
    }

    [Fact]
    public async Task SaveWishlistRequest_ShouldAddWishlistEntityToDatabase()
    {
        // Arrange
        var email = "test@example.com";
        var productId = "123";

        // Act
        var result = await _addToWishlistService.SaveWishlistRequest(email, productId);

        // Assert
        var wishlistEntityFromDb = await _context.Wishlists.FirstOrDefaultAsync();
        Assert.NotNull(wishlistEntityFromDb);
        Assert.Equal(email, wishlistEntityFromDb.Email);
        Assert.Equal(productId, wishlistEntityFromDb.ProductId);
        Assert.True(result);
    }
}