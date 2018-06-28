using KD.Builders.Class;
using System;
using System.Collections.Generic;

namespace KD.Builders
{
    /// <summary>
    /// Used to generate 
    /// </summary>
    public static class BuilderFactory
    {
        private static Dictionary<Type, IBuilder> KnownBuilders { get; set; }

        static BuilderFactory()
        {
            KnownBuilders = new Dictionary<Type, IBuilder>();

            RegisterBasicBuilders();
        }

        /// <summary>
        /// Registers new builders singleton.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static bool TryRegisterBuilder<T>(T builder)
            where T : class, IBuilder
        {
            Type builderType = typeof(T);

            if (!KnownBuilders.ContainsKey(builderType))
            {
                KnownBuilders.Add(builderType, builder);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns <see cref="IBuilder"/> from known type; otherwise null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve<T>()
            where T : class, IBuilder
        {
            Type builderType = typeof(T);
            if (KnownBuilders.TryGetValue(builderType, out IBuilder value))
            {
                return (T)value;
            }

            return null;
        }

        /// <summary>
        /// Here are registered all basic known builders.
        /// </summary>
        private static void RegisterBasicBuilders()
        {
            TryRegisterBuilder<IClassBuilder>(new ClassBuilder());
        }
    }
}
