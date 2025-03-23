using Lucene.Net.Store;
using Directory = Lucene.Net.Store.Directory;

namespace LuceneAdventureWorks.Common.Search;

public static class DirectoryFactory
{
    public static Directory Create(string path)
    {
        var directoryType = Environment.GetEnvironmentVariable("LUCENE_DIRECTORY_TYPE");

        var directory = directoryType?.ToLowerInvariant() switch
        {
            // "ram" => new RAMDirectory(), // TODO: support this, but it doesn't make sense with current code
            "simplefs" => new SimpleFSDirectory(new DirectoryInfo(path)),
            "niofs" => new NIOFSDirectory(new DirectoryInfo(path)),
            "mmap" => new MMapDirectory(new DirectoryInfo(path)),
            null => FSDirectory.Open(new DirectoryInfo(path)), // use default for OS
            _ => throw new InvalidOperationException($"Unknown directory type: {directoryType}")
        };

        Console.WriteLine($"Created {directory.GetType().Name} for {path}");

        return directory;
    }
}
