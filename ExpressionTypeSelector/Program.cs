using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace ExpressionTypeSelector
{
    public class Program
    {
        static void Main(string[] args)
        {
            //var sections = Section.Parse("{5}B{2}A{5}C");
            //var trieType = new TrieTypeMatcher().MatchOn(sections);
            //var expressionType = new ExpressionTypeMatcher().MatchOn(sections);
            //new TypeGenerator().GenerateClasses(100);

            BenchmarkRunner.Run<MatcherBenchmarks>();
        }

        public class MatcherBenchmarks
        {
            private readonly TrieTypeMatcher _trieTypeMatcher = new TrieTypeMatcher();
            private readonly ExpressionTypeMatcher _expressionTypeMatcher = new ExpressionTypeMatcher();
            private static IReadOnlyList<Section> SectionsFor75 => Section.Parse("{5}1234{3}A{2}A{4}123{5}abc");
            private static IReadOnlyList<Section> SectionsFor24 => Section.Parse("{1}C{4}C{5}C");
            private static IReadOnlyList<Section> SectionsFor99 => Section.Parse("{5}B{2}A{5}C");
            private static IReadOnlyList<Section> SectionsFor0 => Section.Parse("{2}abc{5}B{5}C");

            [Benchmark]
            public Type Trie75Lookup() => _trieTypeMatcher.MatchOn(SectionsFor75);

            [Benchmark]
            public Type Expression75Lookup() => _expressionTypeMatcher.MatchOn(SectionsFor75);
            
            [Benchmark]
            public Type Trie24Lookup() => _trieTypeMatcher.MatchOn(SectionsFor24);

            [Benchmark]
            public Type Expression24Lookup() => _expressionTypeMatcher.MatchOn(SectionsFor24);
            
            [Benchmark]
            public Type Trie99Lookup() => _trieTypeMatcher.MatchOn(SectionsFor99);

            [Benchmark]
            public Type Expression99Lookup() => _expressionTypeMatcher.MatchOn(SectionsFor99);
            
            [Benchmark]
            public Type Trie0Lookup() => _trieTypeMatcher.MatchOn(SectionsFor0);

            [Benchmark]
            public Type Expression0Lookup() => _expressionTypeMatcher.MatchOn(SectionsFor0);
        }
    }
}