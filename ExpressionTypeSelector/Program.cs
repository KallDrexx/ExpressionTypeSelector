using System;

namespace ExpressionTypeSelector
{
    class Program
    {
        static void Main(string[] args)
        {
            var sections = Section.Parse("{1}abc{2}def{3}345{4}123{5}abc");
            var trieType = new TrieTypeMatcher().MatchOn(sections);
            var expressionType = new ExpressionTypeMatcher().MatchOn(sections);
        }
    }
}