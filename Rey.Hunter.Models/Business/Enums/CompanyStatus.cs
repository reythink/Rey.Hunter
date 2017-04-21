using Rey.Hunter.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Hunter.Models.Business {
    public enum CompanyStatus {
        [EnglishDescription("Normal")]
        [ChineseDescription("普通")]
        Normal = 1,

        [EnglishDescription("Developing")]
        [ChineseDescription("开发中")]
        Developing = 2,

        [EnglishDescription("Cooperating")]
        [ChineseDescription("客户公司")]
        Cooperating = 3,

        [EnglishDescription("Terminated")]
        [ChineseDescription("终止合作")]
        Terminated = 4,
    }
}
