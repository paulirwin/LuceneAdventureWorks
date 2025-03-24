package org.example;

import org.apache.lucene.analysis.Analyzer;
import org.apache.lucene.analysis.standard.StandardAnalyzer;
import org.apache.lucene.util.Version;

public final class AnalyzerFactory {
    public static Analyzer Create() {
        // TODO: support configurable analyzer types
        return new StandardAnalyzer(Version.LUCENE_48);
    }
}

