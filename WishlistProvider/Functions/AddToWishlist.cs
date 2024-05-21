using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WishlistProvider.Data.Contexts;
using WishlistProvider.Data.Entities;
using WishlistProvider.Models;
using WishlistProvider.Services;

namespace WishlistProvider.Functions;

public class AddToWishlist(ILogger<AddToWishlist> logger, IAddToWishlistService addToWishlistService)
{
    private readonly ILogger<AddToWishlist> _logger = logger;
    private readonly IAddToWishlistService _addToWishlistService = addToWishlistService;

    [Function("AddToWishlist")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
    {
        try
        {
            var wishlistRequest = await _addToWishlistService.UnpackWishlistRequest(req);
            if (wishlistRequest != null)
            {
                var result = await _addToWishlistService.SaveWishlistRequest(wishlistRequest.Email, wishlistRequest.ProductId);
                if (result)
                {
                    return new OkResult();
                }

            }
        

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error : AddToWishlist.Run() :: {ex.Message} ");
        }
        return new BadRequestResult();  
    }

    
}
