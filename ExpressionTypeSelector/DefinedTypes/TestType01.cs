
using System.Collections.Generic;

namespace ExpressionTypeSelector.DefinedTypes
{
    public class TestType01 : ITestType
    {
        public IReadOnlyList<Section> IdentificationPattern => new[]
        {
				new Section(5, "C"),
				new Section(1, "1234"),
				new Section(4, "abc"),

        };
    }
}
