using LuceneAdventureWorks;

if (args.Length == 0)
{
    Console.WriteLine("Usage:");
    Console.WriteLine("  LuceneAdventureWorks <command> [options]");
    Console.WriteLine();
    Console.WriteLine("Commands:");
    Console.WriteLine("  index        Index the AdventureWorks database");
    Console.WriteLine("  loadtest     Load test the index");
    Console.WriteLine("  search       Search the index");
    Console.WriteLine();
    Console.WriteLine("Options:");
    Console.WriteLine("  -p <path>    The path to the index directory");
    return 0;
}

var path = Path.Combine(Directory.GetCurrentDirectory(), "index");
if (args.Contains("-p"))
{
    var indexOptionIndex = Array.IndexOf(args, "-p");
    if (indexOptionIndex + 1 < args.Length)
    {
        path = args[indexOptionIndex + 1];
    }
}

if (!Directory.Exists(path))
{
    Directory.CreateDirectory(path);
}

if (args[0] == "index")
{
    IndexCommand.Run(path);
}
else if (args[0] == "loadtest")
{
    LoadTestCommand.Run(path);
}
else if (args[0] == "search")
{
    SearchCommand.Run(path);
}
else
{
    Console.WriteLine($"Unknown command: {args[0]}");
    return -2;
}

return 0;
