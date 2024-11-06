using Data.Contexts;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace UsersProvider_Rika.Functions;

public class UpdateUser(ILogger<UpdateUser> logger, DataContext context)
{
    private readonly ILogger<UpdateUser> _logger = logger;
    private readonly DataContext _context = context;



    [Function("UpdateUser")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "users/{userId}")] HttpRequest req, string userId)
    {
        try
        {
            var userForm = await req.ReadFromJsonAsync<UserForm>();            

            var user = await _context.Users.FirstAsync(x => x.Id == userId);


            if (user != null )
            {

                user.Address = userForm!.Address;
                user.PostalCode = userForm.PostalCode;
                user.City = userForm.City;
                user.Country = userForm.Country;
                user.FirstName = userForm.City;
                user.LastName = userForm.Country;
                user.ImageUrl = userForm.ImageUrl;


                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return new OkObjectResult(user);

            }
            else
            {
                return new NotFoundObjectResult("User not found");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new BadRequestObjectResult(ex.Message);
        }
    }
}
