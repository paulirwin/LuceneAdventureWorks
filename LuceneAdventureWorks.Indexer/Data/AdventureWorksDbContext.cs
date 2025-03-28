using Microsoft.EntityFrameworkCore;

namespace LuceneAdventureWorks.Indexer.Data;

public class AdventureWorksDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
}
