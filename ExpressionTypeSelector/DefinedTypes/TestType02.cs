
using System.Collections.Generic;

namespace ExpressionTypeSelector.DefinedTypes
{
    public class TestType02 : ITestType
    {
        public IReadOnlyList<Section> IdentificationPattern => new[]
        {
				new Section(2, "A"),
				new Section(4, "C"),

        };
    }
}
