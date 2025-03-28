using System.Collections.Concurrent;
using System.Diagnostics;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Util;
using LuceneAdventureWorks.Common.Search;

namespace LuceneAdventureWorks;

public static class LoadTestCommand
{
    private static readonly string[] _queries =
    [
        "mountain",
        "bike",
        "helmet",
        "gloves",
        "jacket",
        "shorts",
        "shoes",
        "pedals",
        "handlebars",
        "seat",
        "frame",
        "fork",
        "shock",
        "tire",
        "wheel",
        "brake",
        "chain",
        "derailleur",
        "cassette",
        "crank",
        "bottom bracket",
        "headset",
        "stem",
        "saddle",
        "seatpost",
        "grips",
        "pump",
        "tube",
        "patch",
        "lube",
        "cleaner",
        "tool",
        "stand",
        "bag",
        // possibly unrelated terms
        "shirt",
        "pants",
        "hat",
        "hydrogen",
        "oxygen",
        "carbon",
        "nitrogen",
        "silicon",
        "RAM",
        "CPU",
        "hard drive",
        "motherboard",
        "power supply",
        "blueberry",
        "strawberry",
        "raspberry",
        "blackberry",
    ];

    public static void Run(string path)
    {
        using var directory = DirectoryFactory.Create(path);
        using var analyzer = AnalyzerFactory.Create();
        var searcherManager = new SearcherManager(directory, null);

        const int iterations = 100_000;
        Console.WriteLine($"Starting load test of {iterations} iterations");

        var stopwatch = Stopwatch.StartNew();
        var productHits = new ConcurrentDictionary<int, int>();

        Parallel.For(0, iterations, i =>
        {
            var queryParser = new QueryParser(LuceneVersion.LUCENE_48, "Name", analyzer);
            var query = queryParser.Parse(_queries[i % _queries.Length]);
            var searcher = searcherManager.Acquire();
            try
            {
                var topDocs = searcher.Search(query, 100);

                foreach (var scoreDoc in topDocs.ScoreDocs)
                {
                    var doc = searcher.Doc(scoreDoc.Doc);
                    productHits.AddOrUpdate(int.Parse(doc.Get("ProductID")), 1, (_, count) => count + 1);
                }
            }
            finally
            {
                searcherManager.Release(searcher);
            }
        });

        stopwatch.Stop();

        Console.WriteLine($"Finished load test in {stopwatch.Elapsed}");
    }
}
