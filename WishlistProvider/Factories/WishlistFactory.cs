using WishlistProvider.Data.Entities;
using WishlistProvider.Models;

namespace WishlistProvider.Factories;

public class WishlistFactory
{
    public static WishlistResponse Create(WishlistEntity entity)
    {
        try
        {
            return new WishlistResponse
            {
                Email = entity.Email,
                ProductId = entity.ProductId
            };
        }
        catch
        {

        }
        return null!;
    }

    public static IEnumerable<WishlistResponse> Create(List<WishlistEntity> entities)
    {
        try
        {
            var wishlist = new List<WishlistResponse>();

            foreach(var entity in entities)
            {
                wishlist.Add(Create(entity));
            }

            return wishlist;
        }
        catch
        {

        }
        return null!;
    }
}
