
using System.Collections.Generic;

namespace ExpressionTypeSelector.DefinedTypes
{
    public class TestType84 : ITestType
    {
        public IReadOnlyList<Section> IdentificationPattern => new[]
        {
				new Section(5, "1234"),
				new Section(3, "1234"),
				new Section(1, "A"),

        };
    }
}
