using MongoDB.Bson.Serialization.Attributes;
using Rey.Hunter.Models2.Attributes;
using Rey.Hunter.Models2.Enums;
using System.Collections.Generic;

namespace Rey.Hunter.Models2 {
    public class CompanyAddress {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
    }

    [MongoCollection("company")]
    public class Company : AccountModel {
        public string Name { get; set; }

        public CompanyType? Type { get; set; }

        public CompanyStatus? Status { get; set; }

        public List<CompanyAddress> Address { get; set; } = new List<CompanyAddress>();

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
