using Lucene.Net.Analysis;
using Lucene.Net.Index;
using Lucene.Net.Util;
using Directory = Lucene.Net.Store.Directory;

namespace LuceneAdventureWorks.Common.Search;

public static class IndexWriterFactory
{
    public static IndexWriter Create(Directory directory, Analyzer analyzer)
    {
        Console.WriteLine($"Creating IndexWriter for {directory.GetType().Name} and {analyzer.GetType().Name}");
        var config = new IndexWriterConfig(LuceneVersion.LUCENE_48, analyzer);
        // TODO: support other config options
        return new IndexWriter(directory, config);
    }
}
