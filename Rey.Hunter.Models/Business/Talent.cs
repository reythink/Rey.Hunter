using Rey.Mon.Attributes;
using Rey.Mon.Models;
using MongoDB.Bson.Serialization.Attributes;
using Rey.Hunter.Models.Basic;
using System;
using System.Collections.Generic;

namespace Rey.Hunter.Models.Business {
    [MonCollection("bus.talents")]
    public class Talent : AccountModel {
        #region Basic Info

        public List<MonStringNodeModelRef<IndustryNode>> Industries { get; set; } = new List<MonStringNodeModelRef<IndustryNode>>();

        public List<MonStringNodeModelRef<FunctionNode>> Functions { get; set; } = new List<MonStringNodeModelRef<FunctionNode>>();

        public string EnglishName { get; set; }

        public string ChineseName { get; set; }

        public DateTime? Birthday { get; set; }

        public Gender? Gender { get; set; }

        public MaritalStatus? MaritalStatus { get; set; }

        public EducationLevel? EducationLevel { get; set; }

        public Language? Language { get; set; }

        public Nationality? Nationality { get; set; }

        public JobIntension? Intension { get; set; }

        public string Linkedin { get; set; }

        public string CV { get; set; }

        public string Notes { get; set; }

        #endregion Basic Info

        #region Locations

        public List<MonStringNodeModelRef<LocationNode>> CurrentLocations { get; set; } = new List<MonStringNodeModelRef<LocationNode>>();

        public List<MonStringNodeModelRef<LocationNode>> MobilityLocations { get; set; } = new List<MonStringNodeModelRef<LocationNode>>();

        #endregion Locations

        #region Contacts

        [BsonIgnoreIfNull]
        public string Phone { get; set; }

        [BsonIgnoreIfNull]
        public string Mobile { get; set; }

        [BsonIgnoreIfNull]
        public string Email { get; set; }

        [BsonIgnoreIfNull]
        public string QQ { get; set; }

        [BsonIgnoreIfNull]
        public string Wechat { get; set; }

        #endregion Contacts

        public TalentProfileLabel ProfileLabel { get; set; }

        public List<TalentExperience> Experiences { get; set; } = new List<TalentExperience>();

        public List<Attachment> Attachments { get; set; } = new List<Attachment>();
    }

    public class TalentProfileLabel {
        /// <summary>
        /// 跨行业经验
        /// </summary>
        public List<MonStringNodeModelRef<IndustryNode>> CrossIndustries { get; set; } = new List<MonStringNodeModelRef<IndustryNode>>();

        /// <summary>
        /// 跨职能经验
        /// </summary>
        public List<MonStringNodeModelRef<FunctionNode>> CrossFunctions { get; set; } = new List<MonStringNodeModelRef<FunctionNode>>();

        /// <summary>
        /// 跨渠道经验
        /// </summary>
        public List<MonStringNodeModelRef<ChannelNode>> CrossChannels { get; set; } = new List<MonStringNodeModelRef<ChannelNode>>();

        /// <summary>
        /// 跨品类经验 
        /// </summary>
        public List<MonStringNodeModelRef<CategoryNode>> CrossCategories { get; set; } = new List<MonStringNodeModelRef<CategoryNode>>();

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

    public class TalentExperience {
        [BsonIgnoreIfNull]
        public MonStringModelRef<Company> Company { get; set; } = new MonStringModelRef<Company>();

        [BsonIgnoreIfNull]
        public bool? CurrentJob { get; set; }

        [BsonIgnoreIfNull]
        public int? FromYear { get; set; }

        [BsonIgnoreIfNull]
        public int? FromMonth { get; set; }

        [BsonIgnoreIfNull]
        public int? ToYear { get; set; }

        [BsonIgnoreIfNull]
        public int? ToMonth { get; set; }

        [BsonIgnoreIfNull]
        public string Title { get; set; }

        [BsonIgnoreIfNull]
        public string Responsibility { get; set; }

        [BsonIgnoreIfNull]
        public string Grade { get; set; }

        [BsonIgnoreIfNull]
        public string AnnualPackage { get; set; }

        [BsonIgnoreIfNull]
        public string Description { get; set; }

        [BsonIgnoreIfNull]
        public int? BasicSalary { get; set; }

        [BsonIgnoreIfNull]
        public int? BasicSalaryMonths { get; set; }

        [BsonIgnoreIfNull]
        public string Bonus { get; set; }

        [BsonIgnoreIfNull]
        public string Allowance { get; set; }
    }
}
