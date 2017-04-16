using Rey.Hunter.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Hunter.Models.Web.Business {
    public enum Nationality {
        [EnglishDescription("Chinese(Mainland)")]
        [ChineseDescription("中国（大陆）")]
        ChineseMainland = 1,

        [EnglishDescription("Chinese(Hongkong)")]
        [ChineseDescription("中国（香港）")]
        ChineseHongkong = 2,

        [EnglishDescription("Chinese(Taiwan)")]
        [ChineseDescription("中国（台湾）")]
        ChineseTaiwan = 3,

        [EnglishDescription("Singaporean")]
        [ChineseDescription("新加坡")]
        Singaporean = 4,

        [EnglishDescription("Southeast Asian")]
        [ChineseDescription("东南亚")]
        SoutheastAsian = 5,

        [EnglishDescription("Korean")]
        [ChineseDescription("韩国")]
        Korean = 6,

        [EnglishDescription("Japanese")]
        [ChineseDescription("日本")]
        Japanese = 7,

        [EnglishDescription("Indian")]
        [ChineseDescription("印度")]
        Indian = 8,

        [EnglishDescription("Other Asian Countries")]
        [ChineseDescription("其他亚洲国家")]
        OtherAsianCountries = 9,

        [EnglishDescription("American")]
        [ChineseDescription("美国")]
        American = 10,

        [EnglishDescription("Canadian")]
        [ChineseDescription("加拿大")]
        Canadian = 11,

        [EnglishDescription("South American")]
        [ChineseDescription("南美")]
        SouthAmerican = 12,

        [EnglishDescription("French")]
        [ChineseDescription("法国")]
        French = 13,

        [EnglishDescription("German")]
        [ChineseDescription("德国")]
        German = 14,

        [EnglishDescription("Italian")]
        [ChineseDescription("意大利")]
        Italian = 15,

        [EnglishDescription("Other European")]
        [ChineseDescription("其他欧洲国家")]
        OtherEuropean = 16,

        [EnglishDescription("Australian")]
        [ChineseDescription("澳大利亚")]
        Australian = 17,

        [EnglishDescription("African")]
        [ChineseDescription("非洲")]
        African = 18,

        [EnglishDescription("Others")]
        [ChineseDescription("其他")]
        Others = 9999
    }
}
