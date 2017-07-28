using MongoDB.Bson.Serialization.Attributes;

namespace Rey.Hunter.Models2.Business {
    public class TalentExperience {
        [BsonIgnoreIfNull]
        public CompanyRef Company { get; set; }

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
