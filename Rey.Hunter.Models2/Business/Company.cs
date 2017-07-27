using MongoDB.Bson.Serialization.Attributes;
using Rey.Hunter.Models2.Attributes;
using Rey.Hunter.Models2.Data;
using Rey.Hunter.Models2.Enums;
using System.Collections.Generic;

namespace Rey.Hunter.Models2 {
    [MongoCollection("busi.company")]
    public class Company : AccountModel {
        public string Name { get; set; }

        public List<string> Industry { get; set; } = new List<string>();

        public CompanyType? Type { get; set; }

        public CompanyStatus? Status { get; set; }

        public TalentRef HR { get; set; }

        public TalentRef LineManager { get; set; }

        public List<CompanyAddress> Address { get; set; } = new List<CompanyAddress>();

        public List<Attachment> DepartmentStructures { get; set; } = new List<Attachment>();

        public List<Attachment> NameList { get; set; } = new List<Attachment>();

        [BsonIgnoreIfNull]
        public string Introduction { get; set; }

        [BsonIgnoreIfNull]
        public string SalaryStructure { get; set; }

        [BsonIgnoreIfNull]
        public string Culture { get; set; }

        [BsonIgnoreIfNull]
        public string BasicRecruitmentPrinciple { get; set; }
    }
}
