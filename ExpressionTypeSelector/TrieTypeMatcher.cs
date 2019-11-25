using System;
using System.Collections.Generic;
using System.Linq;
using ExpressionTypeSelector.DefinedTypes;

namespace ExpressionTypeSelector
{
    public class TrieTypeMatcher
    {
        private readonly Node _rootNode = new Node();

        public TrieTypeMatcher()
        {
            var implementations = typeof(ITestType).Assembly
                .GetTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => !x.IsInterface)
                .Where(x => typeof(ITestType).IsAssignableFrom(x))
                .Select(x => (ITestType) Activator.CreateInstance(x));

            foreach (var implementation in implementations)
            {
                var currentNode = _rootNode;
                foreach (var section in implementation.IdentificationPattern)
                {
                    if (!currentNode.Children.TryGetValue(section.Number.ToString(), out var nextNode))
                    {
                        nextNode = new Node();
                        currentNode.Children[section.Number.ToString()] = nextNode;
                    }

                    currentNode = nextNode;
                    
                    if (!currentNode.Children.TryGetValue(section.Value, out nextNode))
                    {
                        nextNode = new Node();
                        currentNode.Children[section.Value] = nextNode;
                    }
                    
                    currentNode = nextNode;
                }

                currentNode.MatchingType = implementation.GetType();
            }
        }

        public Type MatchOn(Section[] sections)
        {
            return Search(sections.AsSpan(), _rootNode);
        }

        private static Type Search(Span<Section> sections, Node currentNode)
        {
            if (sections.IsEmpty)
            {
                return null;
            }

            var currentSection = sections[0];
            if (currentNode.Children.TryGetValue(currentSection.Number.ToString(), out var numberNode))
            {
                if (numberNode.Children.TryGetValue(currentSection.Value, out var explicitValueNode))
                {
                    var innerType = Search(sections.Slice(1), explicitValueNode);
                    if (innerType != null)
                    {
                        return innerType;
                    }

                    if (explicitValueNode.MatchingType != null)
                    {
                        return explicitValueNode.MatchingType;
                    }
                }

                if (numberNode.Children.TryGetValue("*", out var wildcardValueNode))
                {
                    var innerType = Search(sections.Slice(1), wildcardValueNode);
                    if (innerType != null)
                    {
                        return innerType;
                    }

                    if (wildcardValueNode.MatchingType != null)
                    {
                        return wildcardValueNode.MatchingType;
                    }
                }
            }

            // Types are only on value nodes, so if we got here then no matches were found in this branch
            return null;
        }
        
        private class Node
        {
            public Dictionary<string, Node> Children { get; } = new Dictionary<string, Node>();
            public Type MatchingType { get; set; }
        }
    }
}