
using System.Collections.Generic;

namespace ExpressionTypeSelector.DefinedTypes
{
    public class TestType6 : ITestType
    {
        public IReadOnlyList<Section> IdentificationPattern => new[]
        {
				new Section(4, "C"),
				new Section(1, "def"),

        };
    }
}
