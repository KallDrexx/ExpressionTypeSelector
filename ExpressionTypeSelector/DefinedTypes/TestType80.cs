
using System.Collections.Generic;

namespace ExpressionTypeSelector.DefinedTypes
{
    public class TestType80 : ITestType
    {
        public IReadOnlyList<Section> IdentificationPattern => new[]
        {
				new Section(1, "B"),
				new Section(4, "A"),
				new Section(5, "A"),

        };
    }
}
