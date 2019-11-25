using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ExpressionTypeSelector.DefinedTypes;

namespace ExpressionTypeSelector
{
    public class ExpressionTypeMatcher
    {
        private readonly Func<Section[], Type> _matchFunction;
        
        public ExpressionTypeMatcher()
        {
            var implementations = typeof(ITestType).Assembly
                .GetTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => !x.IsInterface)
                .Where(x => typeof(ITestType).IsAssignableFrom(x))
                .OrderBy(x => x.Name)
                .Select(x => (ITestType) Activator.CreateInstance(x));

            var countProperty = typeof(Section[])
                .GetProperty("Length");

            var returnTarget = Expression.Label(typeof(Type));
            var sectionParameter = Expression.Parameter(typeof(Section[]), "sections");
            var ifBlocks = new List<Expression>();

            foreach (var implementation in implementations)
            {
                var minLength = implementation.IdentificationPattern.Count;
                
                var lengthCheckLeft = Expression.Property(sectionParameter, countProperty);
                var lengthCheckRight = Expression.Constant(minLength);
                var test = Expression.GreaterThanOrEqual(lengthCheckLeft, lengthCheckRight);
                
                for (var x = 0; x < implementation.IdentificationPattern.Count; x++)
                {
                    var patternSection = implementation.IdentificationPattern[x];
                    var sectionParameterElement = Expression.ArrayIndex(sectionParameter, Expression.Constant(x));

                    var numberCheckLeft = Expression.Property(sectionParameterElement, typeof(Section).GetProperty("Number"));
                    var numberCheckRight = Expression.Constant(patternSection.Number);
                    test = Expression.AndAlso(test, Expression.Equal(numberCheckLeft, numberCheckRight));

                    if (patternSection.Value != "*")
                    {
                        var valueCheckLeft = Expression.Property(sectionParameterElement, typeof(Section).GetProperty("Value"));
                        var valueCheckRight = Expression.Constant(patternSection.Value);

                        test = Expression.AndAlso(test, Expression.Equal(valueCheckLeft, valueCheckRight));
                    }
                }

                var successExpression = Expression.Return(returnTarget, Expression.Constant(implementation.GetType()));
                var ifBlock = Expression.IfThen(test, successExpression);
                ifBlocks.Add(ifBlock);
            }
            
            var allExpressions = ifBlocks.Concat(new[] {Expression.Label(returnTarget, 
                Expression.Constant(null, typeof(Type)))});

            var block = Expression.Block(allExpressions);
            _matchFunction = Expression.Lambda<Func<Section[], Type>>(block, sectionParameter)
                .Compile();
        }

        public Type MatchOn(IReadOnlyList<Section> sections)
        {
            return _matchFunction(sections.ToArray());
        }
    }
}