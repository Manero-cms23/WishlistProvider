using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WishlistProvider.Data.Contexts;
using WishlistProvider.Data.Entities;
using WishlistProvider.Models;

namespace WishlistProvider.Services;

public class AddToWishlistService(ILogger<AddToWishlistService> logger, DataContext context) : IAddToWishlistService
{
    private readonly ILogger<AddToWishlistService> _logger = logger;
    private readonly DataContext _context = context;

    public async Task<WishlistRequest> UnpackWishlistRequest(HttpRequest req)
    {
        try
        {
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            if (!string.IsNullOrEmpty(body))
            {
                var wishlistRequest = JsonConvert.DeserializeObject<WishlistRequest>(body);
                if (wishlistRequest != null)
                {
                    return wishlistRequest;
                }
            }

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error : AddToWishlist.UnpackWishlistRequest() :: {ex.Message} ");
        }
        return null!;
    }

    public async Task<bool> SaveWishlistRequest(string email, string productId)
    {
        try
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(productId))
            {
                var wishlistEntity = new WishlistEntity { Email = email, ProductId = productId };
                if (wishlistEntity != null)
                {
                    _context.Wishlists.Add(wishlistEntity);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error : AddToWishlist.SaveWishlistRequest() :: {ex.Message} ");
        }
        return false;
    }
}
