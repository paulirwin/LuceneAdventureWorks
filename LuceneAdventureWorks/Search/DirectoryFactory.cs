using Lucene.Net.Store;
using Directory = Lucene.Net.Store.Directory;

namespace LuceneAdventureWorks.Search;

public static class DirectoryFactory
{
    public static Directory Create(string path)
    {
        Console.WriteLine($"Creating directory for {path}");

        var directoryType = Environment.GetEnvironmentVariable("LUCENE_DIRECTORY_TYPE");

        return directoryType?.ToLowerInvariant() switch
        {
            // "ram" => new RAMDirectory(), // TODO: support this, but it doesn't make sense with current code
            "simplefs" => new SimpleFSDirectory(new DirectoryInfo(path)),
            "niofs" => new NIOFSDirectory(new DirectoryInfo(path)),
            "mmap" => new MMapDirectory(new DirectoryInfo(path)),
            _ => FSDirectory.Open(new DirectoryInfo(path)), // use default for OS
        };
    }
}
