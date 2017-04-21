using Rey.Mon.Attributes;
using Rey.Mon.Models;
using MongoDB.Bson.Serialization.Attributes;
using Rey.Hunter.Models.Basic;
using System.Collections.Generic;

namespace Rey.Hunter.Models.Business {
    [MonCollection("bus.companies")]
    public class Company : AccountModel {
        public string Name { get; set; }

        public List<MonStringNodeModelRef<IndustryNode>> Industries { get; set; } = new List<MonStringNodeModelRef<IndustryNode>>();

        public CompanyType? Type { get; set; }

        public CompanyStatus? Status { get; set; }

        public List<Contact> Contacts { get; set; } = new List<Contact>();

        public List<Attachment> DepartmentStructures { get; set; } = new List<Attachment>();

        public List<Attachment> NameList { get; set; } = new List<Attachment>();

        public string Introduction { get; set; }

        public string SalaryStructure { get; set; }

        public string Culture { get; set; }

        public string BasicRecruitmentPrinciple { get; set; }
    }

    [MonCollection("bus.companies.groups")]
    public class CompanyGroup : AccountModel {
        public string Name { get; set; }
    }
}
