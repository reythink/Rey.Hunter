using System.Collections.Generic;

namespace Rey.Hunter.Models2.Business {
    public class TalentProfileLabel {
        /// <summary>
        /// 跨行业经验
        /// </summary>
        public List<string> CrossIndustries { get; set; } = new List<string>();

        /// <summary>
        /// 跨职能经验
        /// </summary>
        public List<string> CrossFunctions { get; set; } = new List<string>();

        /// <summary>
        /// 跨渠道经验
        /// </summary>
        public List<string> CrossChannels { get; set; } = new List<string>();

        /// <summary>
        /// 跨品类经验 
        /// </summary>
        public List<string> CrossCategories { get; set; } = new List<string>();

        /// <summary>
        /// 品牌经验
        /// </summary>
        public string BrandExp { get; set; }

        /// <summary>
        /// KA经验
        /// </summary>
        public string KeyAccountExp { get; set; }

        public string Others { get; set; }
    }
}
