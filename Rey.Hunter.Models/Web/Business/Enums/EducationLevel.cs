using Rey.Hunter.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Hunter.Models.Web.Business {
    public enum EducationLevel {
        [EnglishDescription("Bachelor")]
        [ChineseDescription("学士")]
        Bachelor = 1,

        [EnglishDescription("Master")]
        [ChineseDescription("硕士")]
        Master = 2,

        [EnglishDescription("MBA")]
        [ChineseDescription("工商管理")]
        MBA = 3,

        [EnglishDescription("PHD")]
        [ChineseDescription("博士")]
        PHD = 4,

        [EnglishDescription("Diploma")]
        [ChineseDescription("专科")]
        Diploma = 5,

        [EnglishDescription("Below Diploma")]
        [ChineseDescription("低于专科")]
        BelowDiploma = 6
    }
}
