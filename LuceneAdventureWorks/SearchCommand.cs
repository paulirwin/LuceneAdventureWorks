using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Util;
using LuceneAdventureWorks.Data;
using LuceneAdventureWorks.Search;

namespace LuceneAdventureWorks;

public static class SearchCommand
{
    public static void Run(string path)
    {
        using var directory = DirectoryFactory.Create(path);
        using var analyzer = AnalyzerFactory.Create();
        using var reader = DirectoryReader.Open(directory);
        var searcher = new IndexSearcher(reader);
        var queryParser = new QueryParser(LuceneVersion.LUCENE_48, "Name", analyzer);

        Console.WriteLine("Enter a search term or press Enter to exit");

        while (true)
        {
            Console.Write("Search: ");
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                break;
            }

            var query = queryParser.Parse(input);
            var topDocs = searcher.Search(query, 10);

            Console.WriteLine($"Found {topDocs.TotalHits} results");

            foreach (var scoreDoc in topDocs.ScoreDocs)
            {
                var doc = searcher.Doc(scoreDoc.Doc);
                Console.WriteLine($"Product {doc.Get("ProductID")}: {doc.Get("Name")}");
            }

            Console.WriteLine();
        }
    }
}
