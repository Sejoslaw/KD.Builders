using KD.Builders;
using KD.Builders.Class;
using System;

namespace Test_BuildSimpleClass
{
    class Program
    {
        static void Main(string[] args)
        {
            IClassBuilder builder = BuilderFactory.Resolve<IClassBuilder>();

            Type newType = builder.CreateType("NewAwesomeAssembly", "KD.Builders.Dynamic", "MyNewAwesomeType",
                new string[] { "Id", "Name", "Year" },
                new Type[] { typeof(Guid), typeof(string), typeof(int) },
                null);

            Console.WriteLine($"New type created: { newType }");
        }
    }
}
