
using System.Collections.Generic;

namespace ExpressionTypeSelector.DefinedTypes
{
    public class TestType12 : ITestType
    {
        public IReadOnlyList<Section> IdentificationPattern => new[]
        {
				new Section(3, "B"),
				new Section(2, "1234"),
				new Section(4, "A"),

        };
    }
}
