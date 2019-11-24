using System.Collections.Generic;

namespace ExpressionTypeSelector
{
    public class Section
    {
        public int Number { get; }
        public string Value { get; }

        public Section(int number, string value)
        {
            Number = number;
            Value = value;
        }

        public static IReadOnlyList<Section> Parse(string rawString)
        {
            if (string.IsNullOrWhiteSpace(rawString))
            {
                return new Section[0];
            }

            var results = new List<Section>();
            var numberStartIndex = 0;
            var valueStartIndex = 0;

            void SaveSection(int index)
            {
                var numberLength = valueStartIndex - numberStartIndex - 1;
                var number = int.Parse(rawString.Substring(numberStartIndex, numberLength));
                var value = rawString.Substring(valueStartIndex, index - valueStartIndex);
                results.Add(new Section(number, value));
            }

            for (var x = 0; x < rawString.Length; x++)
            {
                if (rawString[x] == '{')
                {
                    if (x > 0)
                    {
                        SaveSection(x);
                    }

                    numberStartIndex = x + 1;
                }
                else if (rawString[x] == '}')
                {
                    valueStartIndex = x + 1;
                }
            }

            SaveSection(rawString.Length);
            return results;
        }
    }
}