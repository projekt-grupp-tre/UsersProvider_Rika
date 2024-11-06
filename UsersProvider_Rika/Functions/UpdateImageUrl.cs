using Data.Contexts;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace UsersProvider_Rika.Functions;

public class UpdateImageUrl(ILogger<UpdateImageUrl> logger, DataContext context)
{
    private readonly ILogger<UpdateImageUrl> _logger = logger;
    private readonly DataContext _context = context;

    [Function("UpdateProfileImg")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "usersprofile/{id}")] HttpRequest req, string id)
    {
        try
        {
            var userProfileModel = req.ReadFromJsonAsync<ImageUrlModel>().Result;

            var user = await _context.Users.FirstAsync(x => x.Id == id);


            if (user != null)
            {
               
                user.ImageUrl = userProfileModel!.ImageUrl;
                await _context.SaveChangesAsync();
                return new OkResult();
            }
            else
            {
                return new NotFoundResult();
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while update image");
            return new BadRequestResult();
        }
    }
}
