using System.Collections.Generic;

namespace Rey.Hunter.Models2.Business {
    public class TalentProfile {
        /// <summary>
        /// 跨行业经验
        /// </summary>
        public List<string> CrossIndustry { get; set; } = new List<string>();

        /// <summary>
        /// 跨职能经验
        /// </summary>
        public List<string> CrossFunction { get; set; } = new List<string>();

        /// <summary>
        /// 跨渠道经验
        /// </summary>
        public List<string> CrossChannel { get; set; } = new List<string>();

        /// <summary>
        /// 跨品类经验 
        /// </summary>
        public List<string> CrossCategory { get; set; } = new List<string>();

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
