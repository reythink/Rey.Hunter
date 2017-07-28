using Rey.Hunter.Models2.Attributes;
using Rey.Hunter.Models2.Enums;
using System.Collections.Generic;
using System.Text;

namespace Rey.Hunter.Models2.Business {
    [MongoCollection("busi.talent")]
    public class Talent : AccountModel {
        public List<string> Industry { get; set; } = new List<string>();
        public List<string> Function { get; set; } = new List<string>();

        public string EnglishName { get; set; }
        public string ChineseName { get; set; }
        public int? BirthYear { get; set; }
        public Gender? Gender { get; set; }
        public Marital? Marital { get; set; }
        public Education? Education { get; set; }
        public Language? Language { get; set; }
        public Nationality? Nationality { get; set; }
        public Intension? Intension { get; set; }
        public string Linkedin { get; set; }
        public string Vita { get; set; }
        public string Notes { get; set; }

        public TalentLocation Location { get; set; } = new TalentLocation();

        public TalentContact Contact { get; set; } = new TalentContact();

        public TalentProfile Profile { get; set; } = new TalentProfile();

        public List<TalentExperience> Experiences { get; set; } = new List<TalentExperience>();

        public List<Attachment> Attachments { get; set; } = new List<Attachment>();
    }
}
