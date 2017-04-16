using Rey.Hunter.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Hunter.Models.Web.Business {
    public enum CompanyType {
        [EnglishDescription("MNC")]
        [ChineseDescription("跨国公司")]
        MNC = 1,

        [EnglishDescription("JV")]
        [ChineseDescription("合资企业")]
        JV = 2,

        [EnglishDescription("Local Private Enterprise")]
        [ChineseDescription("民营企业")]
        LocalPrivateEnterprise = 3,

        [EnglishDescription("SOE")]
        [ChineseDescription("国有企业")]
        SOE = 4,

        [EnglishDescription("NGO")]
        [ChineseDescription("非政府组织")]
        NGO = 5,
    }
}
