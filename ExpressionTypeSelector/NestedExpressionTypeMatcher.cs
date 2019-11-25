using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ExpressionTypeSelector.DefinedTypes;

namespace ExpressionTypeSelector
{
    public class NestedExpressionTypeMatcher
    {
        private readonly Func<Section[], Type> _matchFunction;
        
        public NestedExpressionTypeMatcher()
        {
            var sectionParameter = Expression.Parameter(typeof(Section[]), "sections");
            var inputLengthProperty = typeof(Section[]).GetProperty("Length");
            var returnTarget = Expression.Label(typeof(Type));

            var trie = BuildTrie();
            var expression = FormNodeCheckExpression(trie, 0, returnTarget, sectionParameter, inputLengthProperty);

            var returnStatement = Expression.Label(returnTarget, Expression.Constant(null, typeof(Type)));
            var expressions = Expression.Block(new[] {expression, returnStatement});
            _matchFunction = Expression.Lambda<Func<Section[], Type>>(expressions, sectionParameter).Compile();
        }
        
        public Type MatchOn(Section[] sections)
        {
            return _matchFunction(sections);
        }

        private static Node BuildTrie()
        {
            var implementations = typeof(ITestType).Assembly
                .GetTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => !x.IsInterface)
                .Where(x => typeof(ITestType).IsAssignableFrom(x))
                .Select(x => (ITestType) Activator.CreateInstance(x));

            var root = new Node();
            foreach (var implementation in implementations)
            {
                var currentNode = root;
                foreach (var section in implementation.IdentificationPattern)
                {
                    if (!currentNode.Children.TryGetValue(section.Number.ToString(), out var nextNode))
                    {
                        nextNode = new Node();
                        currentNode.Children[section.Number.ToString()] = nextNode;
                    }

                    currentNode = nextNode;

                    if (section.Value == "*")
                    {
                        if (currentNode.WildcardNode == null)
                        {
                            currentNode.WildcardNode = new Node();
                        }

                        currentNode = currentNode.WildcardNode;
                    }
                    else
                    {
                        if (!currentNode.Children.TryGetValue(section.Value, out nextNode))
                        {
                            nextNode = new Node();
                            currentNode.Children[section.Value] = nextNode;
                        }
                    
                        currentNode = nextNode;
                    }
                }

                currentNode.MatchingType = implementation.GetType();
            }

            return root;
        }

        private static Expression FormNodeCheckExpression(Node currentNode, 
            int sectionIndex, 
            LabelTarget returnTarget,
            Expression sectionParameter,
            PropertyInfo lengthProperty)
        {
            var sectionElement = Expression.ArrayIndex(sectionParameter, Expression.Constant(sectionIndex));
            var ifChecks = new List<Expression>();
            foreach (var (rawNumber, numberNode) in currentNode.Children)
            {
                var valueIfStatements = new List<Expression>();
                
                foreach (var (rawValue, valueNode) in numberNode.Children)
                {
                    var innerExpression = FormNodeCheckExpression(valueNode, 
                        sectionIndex + 1,
                        returnTarget,
                        sectionParameter,
                        lengthProperty);

                    var valueCheckLeft = Expression.Property(sectionElement, typeof(Section).GetProperty("Value"));
                    var valueCheckRight = Expression.Constant(rawValue);
                    var valueCheckTest = Expression.Equal(valueCheckLeft, valueCheckRight);

                    var leafReturn = valueNode.MatchingType != null
                        ? (Expression) Expression.Return(returnTarget, Expression.Constant(valueNode.MatchingType))
                        : Expression.Empty();

                    var valueBlock = Expression.Block(new[] {innerExpression, leafReturn});
                    var valueIfStatement = Expression.IfThen(valueCheckTest, valueBlock);
                    valueIfStatements.Add(valueIfStatement);
                }

                if (numberNode.WildcardNode != null)
                {
                    var innerExpression = FormNodeCheckExpression(numberNode.WildcardNode, 
                        sectionIndex + 1,
                        returnTarget,
                        sectionParameter,
                        lengthProperty);
                    
                    var leafReturn = numberNode.MatchingType != null
                        ? (Expression) Expression.Return(returnTarget, Expression.Constant(numberNode.MatchingType))
                        : Expression.Empty();

                    var valueBlock = Expression.Block(new[] {innerExpression, leafReturn});
                    valueIfStatements.Add(valueBlock);
                }

                var numberCheckLeft = Expression.Property(sectionElement, typeof(Section).GetProperty("Number"));
                var numberCheckRight = Expression.Constant(int.Parse(rawNumber));
                var numberCheckTest = Expression.Equal(numberCheckLeft, numberCheckRight);

                var ifCheck = Expression.IfThen(numberCheckTest, Expression.Block(valueIfStatements));
                ifChecks.Add(ifCheck);
            }
            
            var sectionLengthCheckLeft = Expression.Property(sectionParameter, lengthProperty);
            var sectionLengthCheckRight = Expression.Constant(sectionIndex + 1);
            var sectionLengthTest = Expression.GreaterThanOrEqual(sectionLengthCheckLeft, sectionLengthCheckRight);

            var ifLength = Expression.IfThen(sectionLengthTest, Expression.Block(ifChecks));
            
            return ifLength;
        }
        
        private class Node
        {
            public Dictionary<string, Node> Children { get; } = new Dictionary<string, Node>();
            public Node WildcardNode { get; set; }
            public Type MatchingType { get; set; }
        }
    }
}