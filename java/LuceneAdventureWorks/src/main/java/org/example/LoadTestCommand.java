package org.example;

import org.apache.commons.lang3.time.StopWatch;
import org.apache.lucene.queryparser.classic.ParseException;
import org.apache.lucene.queryparser.classic.QueryParser;
import org.apache.lucene.search.SearcherManager;
import org.apache.lucene.util.Version;

import java.io.IOException;
import java.util.Collections;
import java.util.HashMap;
import java.util.HashSet;
import java.util.stream.IntStream;

public final class LoadTestCommand {
    private static final String[] _queries =
    {
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
        "blackberry"
    };

    public static void run(String path) {
        try (var directory = DirectoryFactory.Create(path);
            var analyzer = AnalyzerFactory.Create();
            var searcherManager = new SearcherManager(directory, null)) {

            final int iterations = 100_000;
            System.out.println("Starting load test of  " + iterations + " iterations");

            var stopwatch = new StopWatch();
            stopwatch.start();
            var productHits = Collections.synchronizedMap(new HashMap<Integer, Integer>());

            IntStream.range(0, iterations)
                .parallel()
                .forEach(i -> {
                    try {
                        var queryParser = new QueryParser(Version.LUCENE_48, "Name", analyzer);
                        var query = queryParser.parse(_queries[i % _queries.length]);
                        var searcher = searcherManager.acquire();
                        try {
                            var topDocs = searcher.search(query, 100);

                            for (var scoreDoc : topDocs.scoreDocs)
                            {
                                var doc = searcher.doc(scoreDoc.doc);
                                var productId = Integer.parseInt(doc.get("ProductID"));
                                productHits.compute(productId, (k, v) -> v == null ? 1 : v + 1);
                            }
                        } finally {
                            searcherManager.release(searcher);
                        }
                    } catch (ParseException | IOException e) {
                        throw new RuntimeException(e);
                    }
                });

            stopwatch.stop();

            System.out.println("Finished load test in " + stopwatch.formatTime());
            System.out.println("Total products hit: " + productHits.size());
        } catch (IOException e) {
            throw new RuntimeException(e);
        }
    }
}
