using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Collections.Generic;
using WishlistProvider.Data.Contexts;
using WishlistProvider.Data.Entities;
using WishlistProvider.Factories;
using WishlistProvider.Migrations;
using WishlistProvider.Models;

namespace WishlistProvider.Functions
{
    public class GetAllFromEmail
    {
        private readonly ILogger<GetAllFromEmail> _logger;
        private readonly DataContext _context;

        public GetAllFromEmail(ILogger<GetAllFromEmail> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Function("GetAllFromEmail")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get",Route ="wishlistprovider")] HttpRequest req)
        {
            try
            {
                var wr = await UnpackWishlistRequest(req);
                if(wr != null)
                {
                    List<WishlistEntity> wishlist = await _context.Wishlists.Where(x => x.Email == wr.Email).ToListAsync<WishlistEntity>();

                    if(wishlist != null)
                    {
                        var wishlistResponses = WishlistFactory.Create(wishlist);

                        return new OkObjectResult(wishlistResponses);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR : GetAllFromEmail.Run() :: {ex.Message}");
            }
            return new BadRequestResult();
        }

        public async Task<WishlistRequest> UnpackWishlistRequest(HttpRequest req)
        {
            try
            {
                var body = await new StreamReader(req.Body).ReadToEndAsync();
                if(!string.IsNullOrEmpty(body))
                {
                    var wishlistRequest = JsonConvert.DeserializeObject<WishlistRequest>(body);
                    if(wishlistRequest != null)
                    {
                        return wishlistRequest;
                    }
                }   
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR : GetAllFromEmail.Run() :: {ex.Message}");
            }
            return null!;
        }



    }
}
