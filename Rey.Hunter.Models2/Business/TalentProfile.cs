using Rey.Hunter.Models2.Data;
using System.Collections.Generic;

namespace Rey.Hunter.Models2.Business {
    public class TalentProfile {
        /// <summary>
        /// 跨行业经验
        /// </summary>
        public List<IndustryRef> CrossIndustry { get; set; } = new List<IndustryRef>();

        /// <summary>
        /// 跨职能经验
        /// </summary>
        public List<FunctionRef> CrossFunction { get; set; } = new List<FunctionRef>();

        /// <summary>
        /// 跨渠道经验
        /// </summary>
        public List<ChannelRef> CrossChannel { get; set; } = new List<ChannelRef>();

        /// <summary>
        /// 跨品类经验 
        /// </summary>
        public List<CategoryRef> CrossCategory { get; set; } = new List<CategoryRef>();

        /// <summary>
        /// 品牌经验
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// KA经验
        /// </summary>
        public string KeyAccount { get; set; }

        public string Others { get; set; }
    }
}
