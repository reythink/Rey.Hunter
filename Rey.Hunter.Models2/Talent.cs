using Rey.Hunter.Models2.Attributes;
using Rey.Hunter.Models2.Enums;
using System.Collections.Generic;
using System.Text;

namespace Rey.Hunter.Models2 {
    [MongoCollection("talent")]
    public class Talent : AccountModel {
        public List<string> Industry { get; set; } = new List<string>();
        public List<string> Function { get; set; } = new List<string>();

        public string EnglishName { get; set; }
        public string ChineseName { get; set; }
        public int? DOB { get; set; }
        public Gender? Gender { get; set; }
        public MaritalStatus? MaritalStatus { get; set; }
        public EducationLevel? EducationLevel { get; set; }
        public Language? Language { get; set; }
        public Nationality? Nationality { get; set; }
        public JobIntension? Intension { get; set; }
        public string Linkedin { get; set; }
        public string CV { get; set; }
        public string Notes { get; set; }

        public TalentLocation Location { get; set; }

        public TalentContact Contact { get; set; }

        public TalentProfileLabel ProfileLabel { get; set; }

        public List<TalentExperience> Experiences { get; set; } = new List<TalentExperience>();

        public List<Attachment> Attachments { get; set; } = new List<Attachment>();
    }
}
