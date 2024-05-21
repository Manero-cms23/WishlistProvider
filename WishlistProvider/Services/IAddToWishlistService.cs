using Microsoft.AspNetCore.Http;
using WishlistProvider.Models;

namespace WishlistProvider.Services
{
    public interface IAddToWishlistService
    {
        Task<bool> SaveWishlistRequest(string email, string productId);
        Task<WishlistRequest> UnpackWishlistRequest(HttpRequest req);
    }
}