using Data.Contexts;
using Data.Entities;
using Data.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.AddScoped<UserService>();
        services.AddDbContext<DataContext>(options =>
        {
            options.UseSqlServer(Environment.GetEnvironmentVariable("USER_IDENTITY_SQL"));
        });


        services.AddIdentityCore<UserEntity>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
            options.User.RequireUniqueEmail = true;
            options.Password.RequiredLength = 6;
        })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<DataContext>()
    .AddSignInManager<SignInManager<UserEntity>>()
    .AddUserManager<UserManager<UserEntity>>()
    .AddDefaultTokenProviders();

        services.AddAuthorization(x =>
        {
            x.AddPolicy("Managers", policy => policy.RequireRole("Managers"));
            x.AddPolicy("Admins", policy => policy.RequireRole("Managers", "Admin"));
            x.AddPolicy("Users", policy => policy.RequireRole("Managers", "Admin", "User"));
        });

        services.AddHttpsRedirection(o => o.HttpsPort = 7238);


    })
    .Build();


using (var scope = host.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roles = ["SuperAdmin", "Manager", "Admin", "User"];
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}


host.Run();