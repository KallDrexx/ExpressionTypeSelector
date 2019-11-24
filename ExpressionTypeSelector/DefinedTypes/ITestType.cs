using System.Collections.Generic;

namespace ExpressionTypeSelector.DefinedTypes
{
    public interface ITestType
    {
        IReadOnlyList<Section> IdentificationPattern { get; }
    }
}