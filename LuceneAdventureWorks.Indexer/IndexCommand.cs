using Lucene.Net.Documents;
using LuceneAdventureWorks.Common.Search;
using LuceneAdventureWorks.Indexer.Data;

namespace LuceneAdventureWorks.Indexer;

public static class IndexCommand
{
    public static void Run(string path)
    {
        using var context = AdventureWorksDbContextFactory.Create();
        using var directory = DirectoryFactory.Create(path);
        using var analyzer = AnalyzerFactory.Create();
        using var writer = IndexWriterFactory.Create(directory, analyzer);

        var products = context.Products.ToList();
        Console.WriteLine($"Found {products.Count} products to index");

        foreach (var product in products)
        {
            var doc = new Document
            {
                new StringField("ProductID", product.ProductID.ToString(), Field.Store.YES),
                new TextField("Name", product.Name, Field.Store.YES),
                new StringField("ProductNumber", product.ProductNumber, Field.Store.YES),
                new Int32Field("ListPrice", (int)Math.Truncate(product.ListPrice * 100), Field.Store.YES), // store as cents to avoid floating point issues
            };

            if (product.Color != null)
            {
                doc.Add(new TextField("Color", product.Color, Field.Store.YES));
            }

            if (product.Size != null)
            {
                doc.Add(new TextField("Size", product.Size, Field.Store.YES));
            }

            if (product.Weight != null)
            {
                doc.Add(new SingleField("Weight", (float)product.Weight.Value, Field.Store.YES));
            }

            if (product.Class != null)
            {
                doc.Add(new TextField("Class", product.Class, Field.Store.YES));
            }

            if (product.Style != null)
            {
                doc.Add(new TextField("Style", product.Style, Field.Store.YES));
            }

            if (product.ProductLine != null)
            {
                doc.Add(new TextField("ProductLine", product.ProductLine, Field.Store.YES));
            }

            // TODO: add more fields
            writer.AddDocument(doc);
        }

        writer.Commit();
        writer.Flush(true, true);
        Console.WriteLine("Indexing complete");
    }
}
