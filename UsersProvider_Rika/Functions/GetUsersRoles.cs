using Data.Contexts;
using Data.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace UsersProvider_Rika.Functions;

public class GetUsersRoles(ILogger<GetUsersRoles> logger, UserService userService, DataContext context)
{
    private readonly ILogger<GetUsersRoles> _logger = logger;
    private readonly UserService _userService = userService;
    private readonly DataContext _context = context;

    [Function("GetUsersRoles")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
    {
        try
        {
            var users = await _context.Users.ToListAsync();

            var result = await _userService.GetAllUsersWithRolesAsync(users);
            return new OkObjectResult(result);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new BadRequestResult();
        }

    }
}
