using Rey.Hunter.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Hunter.Models.Business {
    public enum Language {
        [EnglishDescription("Mandarin")]
        [ChineseDescription("国语")]
        Mandarin = 1,

        [EnglishDescription("Cantonese")]
        [ChineseDescription("广东话")]
        Cantonese = 2,

        [EnglishDescription("English")]
        [ChineseDescription("英语")]
        English = 3,

        [EnglishDescription("French")]
        [ChineseDescription("法语")]
        French = 4,

        [EnglishDescription("German")]
        [ChineseDescription("德语")]
        German = 5,

        [EnglishDescription("Japanese")]
        [ChineseDescription("日语")]
        Japanese = 6,

        [EnglishDescription("Korean")]
        [ChineseDescription("韩语")]
        Korean = 7,

        [EnglishDescription("Others")]
        [ChineseDescription("其他")]
        Others = 9999
    }
}
