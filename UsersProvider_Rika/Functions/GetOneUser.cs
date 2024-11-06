using Data.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace UsersProvider_Rika.Functions;

public class GetOneUser(ILogger<GetOneUser> logger, DataContext context)
{
    private readonly ILogger<GetOneUser> _logger = logger;
    private readonly DataContext _context = context;

    [Function("GetOneUser")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users/{id}")] HttpRequest req, string id)
    {

        try
        {
            var user = await _context.Users.FirstAsync(x => x.Id == id);
            if (user == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(user);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }


}
