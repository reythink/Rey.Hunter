using Rey.Hunter.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Hunter.Models.Web.Business {
    public enum Gender {
        [EnglishDescription("Male")]
        [ChineseDescription("男")]
        Male = 1,

        [EnglishDescription("Female")]
        [ChineseDescription("女")]
        Female = 2
    }
}
