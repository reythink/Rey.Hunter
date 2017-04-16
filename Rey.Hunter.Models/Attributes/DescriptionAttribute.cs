using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Hunter.Models.Attributes {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class DescriptionAttribute : Attribute {
        public string Description { get; }
        public string Language { get; }

        public DescriptionAttribute(string description, string language) {
            this.Description = description;
            this.Language = language;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
    public class EnglishDescriptionAttribute : DescriptionAttribute {
        public EnglishDescriptionAttribute(string description)
            : base(description, "en-us") {
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
    public class ChineseDescriptionAttribute : DescriptionAttribute {
        public ChineseDescriptionAttribute(string description)
            : base(description, "zh-cn") {
        }
    }
}
