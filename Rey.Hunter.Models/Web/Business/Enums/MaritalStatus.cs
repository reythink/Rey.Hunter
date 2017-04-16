using Rey.Hunter.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Hunter.Models.Web.Business {
    public enum MaritalStatus {
        [EnglishDescription("Single")]
        [ChineseDescription("单身")]
        Single = 1,

        [EnglishDescription("Married")]
        [ChineseDescription("已婚")]
        Married = 2,

        [EnglishDescription("Married With Child")]
        [ChineseDescription("已婚已育")]
        MarriedWithChild = 3
    }
}
