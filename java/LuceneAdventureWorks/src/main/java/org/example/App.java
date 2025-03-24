package org.example;

import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.Arrays;

/**
 * Hello world!
 *
 */
public class App 
{
    public static void main( String[] args ) {
        if (args.length == 0) {
            System.out.println("Usage:");
            System.out.println("  LuceneAdventureWorks.jar <command> [options]");
            System.out.println();
            System.out.println("Commands:");
            System.out.println("  loadtest     Load test the index");
            System.out.println("  search       Search the index");
            System.out.println();
            System.out.println("Options:");
            System.out.println("  -p <path>    The path to the index directory");
            return;
        }

        var path = Paths.get("index").toString();
        if (Arrays.asList(args).contains("-p")) {
            var indexOptionIndex = Arrays.binarySearch(args, "-p");
            if (indexOptionIndex + 1 < args.length)
            {
                path = args[indexOptionIndex + 1];
            }
        }

        if (!Files.exists(Paths.get(path))) {
            System.out.println("The index directory does not exist: " + path);
        }

        LoadTestCommand.run(path);
    }
}
