
using System.Collections.Generic;

namespace ExpressionTypeSelector.DefinedTypes
{
    public class TestType09 : ITestType
    {
        public IReadOnlyList<Section> IdentificationPattern => new[]
        {
				new Section(3, "abc"),
				new Section(5, "B"),

        };
    }
}
