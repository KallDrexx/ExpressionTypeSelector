using System;

namespace ExpressionTypeSelector
{
    class Program
    {
        static void Main(string[] args)
        {
            var sections = Section.Parse("{5}1234{3}A{2}A{4}123{5}abc");
            var trieType = new TrieTypeMatcher().MatchOn(sections);
            var expressionType = new ExpressionTypeMatcher().MatchOn(sections);

            //new TypeGenerator().GenerateClasses(100);
        }
    }
}