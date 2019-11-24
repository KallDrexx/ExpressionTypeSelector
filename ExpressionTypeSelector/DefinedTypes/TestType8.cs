
using System.Collections.Generic;

namespace ExpressionTypeSelector.DefinedTypes
{
    public class TestType8 : ITestType
    {
        public IReadOnlyList<Section> IdentificationPattern => new[]
        {
				new Section(1, "def"),
				new Section(4, "abc"),
				new Section(3, "1234"),

        };
    }
}
