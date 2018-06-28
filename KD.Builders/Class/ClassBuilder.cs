using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace KD.Builders.Class
{
    /// <summary>
    /// Implementation of <see cref="IClassBuilder"/>.
    /// For more information see: <see cref="IClassBuilder"/>.
    /// </summary>
    internal class ClassBuilder : IClassBuilder
    {
        public Type CreateType(string assemblyName, string moduleName, string typeName, IDictionary<string, Type> properties, Type parentClass = null)
        {
            return this.CreateType(assemblyName, moduleName, typeName, properties.Keys, properties.Values, parentClass);
        }

        public Type CreateType(string assemblyName, string moduleName, string typeName, IEnumerable<string> properties, IEnumerable<Type> propertyTypes, Type parentClass = null)
        {
            int propCount = properties.Count();
            int propTypeCount = propertyTypes.Count();

            if (propCount != propTypeCount)
            {
                throw new IndexOutOfRangeException(
                    $"Number of elements in Properties [{ propCount }] is different from number of elements in Property Types [{ propTypeCount }]");
            }

            TypeBuilder typeBuilder = this.GetTypeBuilder(assemblyName, moduleName, typeName, parentClass);
            ConstructorBuilder constructorBuilder = typeBuilder.DefineDefaultConstructor(
                MethodAttributes.Public |
                MethodAttributes.SpecialName |
                MethodAttributes.RTSpecialName);

            for (int i = 0; i < propCount; ++i)
            {
                string propName = properties.ElementAt(i);
                Type propType = propertyTypes.ElementAt(i);

                this.CreateProperty(typeBuilder, propName, propType);
            }

            TypeInfo createdTypeInfo = typeBuilder.CreateTypeInfo();
            Type createdType = createdTypeInfo.AsType();

            return createdType;
        }

        private void CreateProperty(TypeBuilder typeBuilder, string propName, Type propType)
        {
            // Generate internal field for property.
            FieldBuilder fieldBuilder = typeBuilder.DefineField("_" + propName, propType, FieldAttributes.Private);
            // Generate property definition.
            PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(propName, PropertyAttributes.HasDefault, propType, Type.EmptyTypes);

            // Generate GET method
            MethodBuilder getMethodBuilder = typeBuilder.DefineMethod("get_" + propName,
                MethodAttributes.Public |
                MethodAttributes.SpecialName |
                MethodAttributes.HideBySig,
                propType, Type.EmptyTypes);
            ILGenerator getMethodILGenerator = getMethodBuilder.GetILGenerator();
            getMethodILGenerator.Emit(OpCodes.Ldarg_0);
            getMethodILGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
            getMethodILGenerator.Emit(OpCodes.Ret);

            // Generate SET method
            MethodBuilder setMethodBuilder = typeBuilder.DefineMethod("set_" + propName,
                MethodAttributes.Public |
                MethodAttributes.SpecialName |
                MethodAttributes.HideBySig,
                null, new Type[] { propType });
            ILGenerator setMethodILGenerator = setMethodBuilder.GetILGenerator();
            Label modifyProperty = setMethodILGenerator.DefineLabel();
            Label exitSet = setMethodILGenerator.DefineLabel();

            setMethodILGenerator.MarkLabel(modifyProperty);
            setMethodILGenerator.Emit(OpCodes.Ldarg_0);
            setMethodILGenerator.Emit(OpCodes.Ldarg_1);
            setMethodILGenerator.Emit(OpCodes.Stfld, fieldBuilder);

            setMethodILGenerator.Emit(OpCodes.Nop);
            setMethodILGenerator.MarkLabel(exitSet);
            setMethodILGenerator.Emit(OpCodes.Ret);

            // Attach methods to property.
            propertyBuilder.SetGetMethod(getMethodBuilder);
            propertyBuilder.SetSetMethod(setMethodBuilder);
        }

        private TypeBuilder GetTypeBuilder(string assemblyName, string moduleName, string typeName, Type parentClass)
        {
            var an = new AssemblyName(assemblyName);
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(moduleName);
            TypeBuilder typeBuilder = moduleBuilder.DefineType(typeName,
                TypeAttributes.Public |
                TypeAttributes.Class |
                TypeAttributes.AutoClass |
                TypeAttributes.AnsiClass |
                TypeAttributes.BeforeFieldInit |
                TypeAttributes.AutoLayout, parentClass);
            return typeBuilder;
        }
    }
}
