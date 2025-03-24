package org.example;

import org.apache.lucene.store.*;

import java.io.File;
import java.io.IOException;
import java.util.Locale;

public final class DirectoryFactory {
    public static Directory Create(String path) throws IOException {
        var directoryType = System.getenv("LUCENE_DIRECTORY_TYPE");

        Directory directory;
        if (directoryType == null) {
            directory = FSDirectory.open(new File(path));
        } else {
            switch (directoryType.toLowerCase(Locale.ROOT)) {
                case "simplefs":
                    directory = new SimpleFSDirectory(new File(path));
                    break;
                case "niofs":
                    directory = new NIOFSDirectory(new File(path));
                    break;
                case "mmap":
                    directory = new MMapDirectory(new File(path));
                    break;
                default:
                    throw new java.lang.IllegalArgumentException("Unknown directory type: " + directoryType);
            }
        }

        System.out.println("Created " + directory.getClass().getName() + " for " + path);

        return directory;
    }
}
