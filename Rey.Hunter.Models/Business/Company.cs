using Rey.Mon.Attributes;
using Rey.Mon.Models;
using MongoDB.Bson.Serialization.Attributes;
using Rey.Hunter.Models.Basic;
using System.Collections.Generic;
using Rey.Hunter.Models.Business.Enums;

namespace Rey.Hunter.Models.Business {
    [MonCollection("bus.companies")]
    public class Company : AccountModel {
        [BsonIgnoreIfNull]
        public DataSource? Source { get; set; }

        public string Name { get; set; }

        public List<MonStringNodeModelRef<IndustryNode>> Industries { get; set; } = new List<MonStringNodeModelRef<IndustryNode>>();

        public CompanyType? Type { get; set; }

        public CompanyStatus? Status { get; set; }

        public MonStringModelRef<Talent> HR { get; set; }

        public MonStringModelRef<Talent> LineManager { get; set; }

        public List<Contact> Contacts { get; set; } = new List<Contact>();

        public List<Attachment> DepartmentStructures { get; set; } = new List<Attachment>();

        public List<Attachment> NameList { get; set; } = new List<Attachment>();

        public string Introduction { get; set; }

        public string SalaryStructure { get; set; }

        public string Culture { get; set; }

        public string BasicRecruitmentPrinciple { get; set; }
    }
}
