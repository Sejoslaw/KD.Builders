using System;
using System.Collections.Generic;

namespace KD.Builders
{
    /// <summary>
    /// Contains definition for ClassBuilder which is used to dynamically build classes.
    /// </summary>
    public interface IClassBuilder : IBuilder
    {
        /// <summary>
        /// Returns new created <see cref="Type"/> from given parameters.
        /// </summary>
        /// <param name="assemblyName"> Name of the dynamic assembly. </param>
        /// <param name="moduleName"> Name of the module in which this <see cref="Type"/> will be stored. </param>
        /// <param name="typeName"> Name of the new <see cref="Type"/>. </param>
        /// <param name="properties"> Properties which will be injected to new <see cref="Type"/>. </param>
        /// <param name="parentClass"> Optional parent <see cref="Type"/>. </param>
        /// <returns></returns>
        Type CreateType(string assemblyName, string moduleName, string typeName, IDictionary<string, Type> properties, Type parentClass = null);
        /// <summary>
        /// Returns new created <see cref="Type"/> from given parameters.
        /// </summary>
        /// <param name="assemblyName"> Name of the dynamic assembly. </param>
        /// <param name="moduleName"> Name of the module in which this <see cref="Type"/> will be stored. </param>
        /// <param name="typeName"> Name of the new <see cref="Type"/>. </param>
        /// <param name="properties"> Names of the properties which will be created inside new <see cref="Type"/>. </param>
        /// <param name="propertyTypes"> Types of the properties which will be created inside new <see cref="Type"/>. </param>
        /// <param name="parentClass"> Optional parent <see cref="Type"/>. </param>
        /// <returns></returns>
        Type CreateType(string assemblyName, string moduleName, string typeName, IEnumerable<string> properties, IEnumerable<Type> propertyTypes, Type parentClass = null);
    }
}
