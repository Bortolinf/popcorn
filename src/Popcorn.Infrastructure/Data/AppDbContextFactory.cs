using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Popcorn.Infrastructure.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseMySql(
            "Server=127.0.0.1;Database=popcorn;User=root;Password=;SslMode=None;",
            new MariaDbServerVersion(new Version(10, 4, 28))
        );

        return new AppDbContext(optionsBuilder.Options);
    }
}
