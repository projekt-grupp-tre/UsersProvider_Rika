

using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        //optionsBuilder.UseSqlServer("Server=tcp:sql-iidentity.database.windows.net,1433;Initial Catalog=sql-iidentity;Persist Security Info=False;User ID=SqlAdmin;Password=Bytmig123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        optionsBuilder.UseSqlServer("Server=tcp:elias-rika-server.database.windows.net,1433;Initial Catalog=RikaUserDB;Persist Security Info=False;User ID=RikaAdmin;Password=Bytmig123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

        return new DataContext(optionsBuilder.Options);
    }
}
