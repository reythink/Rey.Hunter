using Rey.Hunter.Models.Attributes;
using Rey.Hunter.Models.Web.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Rey.Hunter {
    public static class EnumExtensions {
        public static string EnumDesc<T>(this T value, string lang = null) {
            if (value == null)
                return "Unknown";

            var type = typeof(T);
            if (type.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                type = type.GenericTypeArguments[0];
            }

            if (!type.GetTypeInfo().IsEnum)
                throw new InvalidOperationException("Not a enum type!");

            var name = Enum.GetName(type, value);
            if (string.IsNullOrEmpty(name))
                throw new InvalidOperationException("Name of vaue if null or empty!");

            var field = type.GetTypeInfo().GetField(name);
            if (field == null)
                throw new InvalidOperationException("Cannot find field of value!");

            var desc = field.GetCustomAttributes<DescriptionAttribute>().FirstOrDefault(x => x.Language.Equals(lang ?? "en-us", StringComparison.CurrentCultureIgnoreCase));
            if (desc != null && !string.IsNullOrEmpty(desc.Description))
                return desc.Description;

            return name;
        }
    }
}
