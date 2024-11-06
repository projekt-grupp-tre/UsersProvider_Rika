using Data.Contexts;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace UsersProvider_Rika.Functions
{
    public class UpdateUserRoles(ILogger<UpdateUserRoles> logger, DataContext context)
    {
        private readonly ILogger<UpdateUserRoles> _logger = logger;
        private readonly DataContext _context = context;

        

        [Function("UpdateUserRoles")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "usersrole/{userId}")] HttpRequest req, string userId)
        {
            try
            {
                var roleForm = req.ReadFromJsonAsync<RoleForm>().Result;

                if (roleForm == null)
                {
                    return new BadRequestObjectResult("Invalid request");
                }
                else
                {
                    var user = _context.Users.FirstOrDefaultAsync(u => u.Id == roleForm.UserId);


                    if (user != null)
                    {
                        var userRoles = await _context.UserRoles
                        .Where(ur => ur.UserId == userId)
                        .ToListAsync();

                        string usersRoleID = _context.Roles.FirstOrDefault(r => r.Name == "User")!.Id;
                        string adminRoleID = _context.Roles.FirstOrDefault(r => r.Name == "Admin")!.Id;
                        string managerRoleID = _context.Roles.FirstOrDefault(r => r.Name == "Manager")!.Id;
                        string SuperAdminRoleID = _context.Roles.FirstOrDefault(r => r.Name == "SuperAdmin")!.Id;

                        if (roleForm.IsUser)
                        {

                            var addUserRole = new IdentityUserRole<string>
                            {
                                UserId = roleForm.UserId,
                                RoleId = usersRoleID
                            };
                            if (!userRoles.Any(ur => ur.RoleId == addUserRole.RoleId))
                            {
                                _context.UserRoles.Add(addUserRole);
                            }
                        }
                        else if (roleForm.IsUser == false && userRoles.Any(ur => ur.RoleId == usersRoleID))
                        {
                            var removeUserRole = userRoles.FirstOrDefault(ur => ur.RoleId == usersRoleID);
                            _context.UserRoles.Remove(removeUserRole!);
                        }
                        if (roleForm.IsAdmin)
                        {

                            var addAdminRole = new IdentityUserRole<string>
                            {
                                UserId = roleForm.UserId,
                                RoleId = adminRoleID
                            };
                            if (!userRoles.Any(ur => ur.RoleId == addAdminRole.RoleId))
                            {
                                _context.UserRoles.Add(addAdminRole);
                            }
                        }
                        else if (roleForm.IsAdmin == false && userRoles.Any(ur => ur.RoleId == adminRoleID))
                        {
                            var removeAdminRole = userRoles.FirstOrDefault(ur => ur.RoleId == adminRoleID);
                            _context.UserRoles.Remove(removeAdminRole!);
                        }
                        if (roleForm.IsSuperAdmin)
                        {

                            var addAdminRole = new IdentityUserRole<string>
                            {
                                UserId = roleForm.UserId,
                                RoleId = adminRoleID
                            };
                            if (!userRoles.Any(ur => ur.RoleId == addAdminRole.RoleId))
                            {
                                _context.UserRoles.Add(addAdminRole);
                            }
                        }
                        else if (roleForm.IsSuperAdmin == false && userRoles.Any(ur => ur.RoleId == adminRoleID))
                        {
                            var removeAdminRole = userRoles.FirstOrDefault(ur => ur.RoleId == adminRoleID);
                            _context.UserRoles.Remove(removeAdminRole!);
                        }

                        if (roleForm.IsManager)
                        {

                            var addManagerRole = new IdentityUserRole<string>
                            {
                                UserId = roleForm.UserId,
                                RoleId = managerRoleID
                            };
                            if (!userRoles.Any(ur => ur.RoleId == addManagerRole.RoleId))
                            {
                                _context.UserRoles.Add(addManagerRole);
                            }
                        }
                        else if (roleForm.IsManager == false && userRoles.Any(ur => ur.RoleId == managerRoleID))
                        {
                            var removeManagerRole = userRoles.FirstOrDefault(ur => ur.RoleId == managerRoleID);
                            _context.UserRoles.Remove(removeManagerRole!);
                        }

                        await _context.SaveChangesAsync();

                        var newUserRolesList = await _context.UserRoles
                       .Where(ur => ur.UserId == roleForm.UserId)
                       .ToListAsync();
                        if (userRoles != null)
                        {
                            return new OkObjectResult(newUserRolesList);
                        }
                        else
                        {
                            return new NotFoundResult();
                        }

                    }
                }
                return new BadRequestResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while update user's role");
                return new BadRequestResult();
            }
        }
    }
}
