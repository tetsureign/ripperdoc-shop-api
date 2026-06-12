using Microsoft.EntityFrameworkCore;
using RipperdocShop.Api.Data;

namespace RipperdocShop.Tests.TestUtils;

public static class TestDbContextFactory
{
    public static ApplicationDbContext CreateInMemoryDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        return new ApplicationDbContext(options, null!);
    }
}
