
using System.Collections.Generic;

namespace ExpressionTypeSelector.DefinedTypes
{
    public class TestType62 : ITestType
    {
        public IReadOnlyList<Section> IdentificationPattern => new[]
        {
				new Section(4, "A"),
				new Section(5, "1234"),
				new Section(1, "C"),

        };
    }
}
