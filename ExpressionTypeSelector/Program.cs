using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace ExpressionTypeSelector
{
    public class Program
    {
        static void Main(string[] args)
        {
            //var sections = Section.Parse("{1}C{4}C{5}C");
            //var trieType = new TrieTypeMatcher().MatchOn(sections);
            //var expressionType = new ExpressionTypeMatcher().MatchOn(sections);
            //var nestedExpressionType = new NestedExpressionTypeMatcher().MatchOn(sections);
            //new TypeGenerator().GenerateClasses(100);
            

            BenchmarkRunner.Run<MatcherBenchmarks>();
        }

        public class MatcherBenchmarks
        {
            private readonly TrieTypeMatcher _trieTypeMatcher = new TrieTypeMatcher();
            private readonly SimpleExpressionTypeMatcher _simpleExpressionTypeMatcher = new SimpleExpressionTypeMatcher();
            private readonly NestedExpressionTypeMatcher _nestedExpressionTypeMatcher = new NestedExpressionTypeMatcher();
            private static Section[] SectionsFor75 => Section.Parse("{5}1234{3}A{2}A{4}123{5}abc").ToArray();
            private static Section[] SectionsFor24 => Section.Parse("{1}C{4}C{5}C").ToArray();
            private static Section[] SectionsFor99 => Section.Parse("{5}B{2}A{5}C").ToArray();
            private static Section[] SectionsFor0 => Section.Parse("{2}abc{5}B{5}C").ToArray();

            [Benchmark]
            public Type Trie75Lookup() => _trieTypeMatcher.MatchOn(SectionsFor75);
            
            [Benchmark]
            public Type Trie24Lookup() => _trieTypeMatcher.MatchOn(SectionsFor24);
            
            [Benchmark]
            public Type Trie99Lookup() => _trieTypeMatcher.MatchOn(SectionsFor99);
            
            [Benchmark]
            public Type Trie0Lookup() => _trieTypeMatcher.MatchOn(SectionsFor0);

            [Benchmark]
            public Type SimpleExpr75Lookup() => _simpleExpressionTypeMatcher.MatchOn(SectionsFor75);

            [Benchmark]
            public Type SimpleExpr24Lookup() => _simpleExpressionTypeMatcher.MatchOn(SectionsFor24);

            [Benchmark]
            public Type SimpleExpr99Lookup() => _simpleExpressionTypeMatcher.MatchOn(SectionsFor99);

            [Benchmark]
            public Type SimpleExpr0Lookup() => _simpleExpressionTypeMatcher.MatchOn(SectionsFor0);
            
            [Benchmark]
            public Type NestedExpr75Lookup() => _nestedExpressionTypeMatcher.MatchOn(SectionsFor75);

            [Benchmark]
            public Type NestedExpr24Lookup() => _nestedExpressionTypeMatcher.MatchOn(SectionsFor24);

            [Benchmark]
            public Type NestedExpr99Lookup() => _nestedExpressionTypeMatcher.MatchOn(SectionsFor99);

            [Benchmark]
            public Type NestedExpr0Lookup() => _nestedExpressionTypeMatcher.MatchOn(SectionsFor0);
        }
    }
}