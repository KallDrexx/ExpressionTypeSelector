using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ExpressionTypeSelector
{
    public class TypeGenerator
    {
        private const int MaxNumber = 5;
        private readonly Random _random = new Random();
        private readonly string[] Values = new[] {"abc", "def", "1234", "A", "B", "C"};

        public void GenerateClasses(int count)
        {
            var directory = Path.Combine(AppContext.BaseDirectory, "classes");
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }

            Directory.CreateDirectory(directory);
            
            for (var x = 0; x < count; x++)
            {
                var contents = GenerateText(x);
                var path = Path.Combine(directory, $"TestType{x}.cs");
                File.WriteAllText(path, contents);
            }
        }

        private string GenerateText(int typeNumber)
        {
            const string template = @"
using System.Collections.Generic;

namespace ExpressionTypeSelector.DefinedTypes
{
    public class TestType!!NUMBER!! : ITestType
    {
        public IReadOnlyList<Section> IdentificationPattern => new[]
        {
!!SECTIONS!!
        };
    }
}
";
            var numberOfSections = _random.Next(2, 4);
            var numbersUsed = new HashSet<int>();
            var sections = new StringBuilder();
            for (var x = 0; x < numberOfSections; x++)
            {
                var number = _random.Next(1, MaxNumber + 1);
                while (numbersUsed.Contains(number))
                {
                    number =_random.Next(1, MaxNumber + 1);
                }
                
                var value = Values[_random.Next(0, Values.Length)];
                sections.AppendFormat("\t\t\t\tnew Section({0}, \"{1}\"),\n", number, value);
                numbersUsed.Add(number);
            }

            return template.Replace("!!SECTIONS!!", sections.ToString())
                .Replace("!!NUMBER!!", typeNumber.ToString());
        }
    }
}