using System.Collections.Generic;

namespace ExpressionTypeSelector.DefinedTypes
{
    public class TestType2 : ITestType
    {
        public IReadOnlyList<Section> IdentificationPattern => new[]
        {
            new Section(1, "abc"),
            new Section(2, "def"),
            new Section(3, "*"),
            new Section(4, "B"),
        };
    }
}