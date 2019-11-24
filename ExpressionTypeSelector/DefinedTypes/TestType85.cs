
using System.Collections.Generic;

namespace ExpressionTypeSelector.DefinedTypes
{
    public class TestType85 : ITestType
    {
        public IReadOnlyList<Section> IdentificationPattern => new[]
        {
				new Section(3, "1234"),
				new Section(4, "1234"),
				new Section(2, "def"),

        };
    }
}
