using System;
using System.Reflection;

namespace Rey.Mon.Attributes {
    [AttributeUsage(AttributeTargets.Class)]
    public class MonCollectionAttribute : Attribute {
        public string Name { get; }

        public MonCollectionAttribute(string name) {
            this.Name = name;
        }

        public static string GetName(Type type) {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return type.GetTypeInfo().GetCustomAttribute<MonCollectionAttribute>()?.Name;
        }

        public static string GetName<TModel>() {
            return GetName(typeof(TModel));
        }
    }
}
