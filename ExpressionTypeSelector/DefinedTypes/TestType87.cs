
using System.Collections.Generic;

namespace ExpressionTypeSelector.DefinedTypes
{
    public class TestType87 : ITestType
    {
        public IReadOnlyList<Section> IdentificationPattern => new[]
        {
				new Section(3, "A"),
				new Section(1, "def"),
				new Section(2, "B"),

        };
    }
}
