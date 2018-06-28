using KD.Builders;
using System;

namespace Test_BuildSimpleClass
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get Class Builder
            IClassBuilder builder = BuilderFactory.Resolve<IClassBuilder>();

            // Create new Type
            Type newType = builder.CreateType("NewAwesomeAssembly", "KD.Builders.Dynamic", "MyNewAwesomeType",
                new string[] { "Id", "Name", "Year" },
                new Type[] { typeof(Guid), typeof(string), typeof(int) },
                null);

            // Create sample object and set it's values.
            dynamic inst = Activator.CreateInstance(newType);
            inst.Id = Guid.NewGuid();
            inst.Name = "Krzysztof";
            inst.Year = 1234567890;

            Console.WriteLine($"New type created: { newType }");
        }
    }
}
