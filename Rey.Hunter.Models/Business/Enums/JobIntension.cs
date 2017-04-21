using Rey.Hunter.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Hunter.Models.Business {
    public enum JobIntension {
        [EnglishDescription("Very Active")]
        [ChineseDescription("非常积极")]
        VeryActive = 1,

        [EnglishDescription("Semi Active")]
        [ChineseDescription("比较积极")]
        SemiActive = 2,

        [EnglishDescription("Semi Passive")]
        [ChineseDescription("比较消极")]
        SemiPassive = 3,

        [EnglishDescription("Very Passive")]
        [ChineseDescription("非常消极")]
        VeryPassive = 4,

        [EnglishDescription("Pregnant")]
        [ChineseDescription("怀孕")]
        Pregnant = 5,
    }
}
