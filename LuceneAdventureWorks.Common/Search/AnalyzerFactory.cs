using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Util;

namespace LuceneAdventureWorks.Common.Search;

public static class AnalyzerFactory
{
    public static Analyzer Create()
    {
        // TODO: support configurable analyzer types
        return new StandardAnalyzer(LuceneVersion.LUCENE_48);
    }
}
