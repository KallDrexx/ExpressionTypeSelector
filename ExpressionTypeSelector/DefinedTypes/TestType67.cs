
using System.Collections.Generic;

namespace ExpressionTypeSelector.DefinedTypes
{
    public class TestType67 : ITestType
    {
        public IReadOnlyList<Section> IdentificationPattern => new[]
        {
				new Section(4, "abc"),
				new Section(3, "C"),
				new Section(1, "B"),

        };
    }
}
