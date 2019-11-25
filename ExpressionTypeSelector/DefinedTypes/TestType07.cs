
using System.Collections.Generic;

namespace ExpressionTypeSelector.DefinedTypes
{
    public class TestType07 : ITestType
    {
        public IReadOnlyList<Section> IdentificationPattern => new[]
        {
				new Section(4, "B"),
				new Section(2, "abc"),
				new Section(3, "1234"),

        };
    }
}
