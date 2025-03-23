using Microsoft.EntityFrameworkCore;

namespace LuceneAdventureWorks.Indexer.Data;

public static class AdventureWorksDbContextFactory
{
    public static AdventureWorksDbContext Create()
    {
        var connectionString = Environment.GetEnvironmentVariable("ADVENTUREWORKS_CONNECTIONSTRING");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string is required to run the application. Please set the ADVENTUREWORKS_CONNECTIONSTRING environment variable.");
        }

        var options = new DbContextOptionsBuilder<AdventureWorksDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new AdventureWorksDbContext(options);
    }
}
