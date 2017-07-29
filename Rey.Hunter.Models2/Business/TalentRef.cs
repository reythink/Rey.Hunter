using System;

namespace Rey.Hunter.Models2.Business {
    public class TalentRef : ModelRef {
        public string EnglishName { get; set; }
        public string ChineseName { get; set; }
        public TalentContact Contact { get; set; }

        public TalentRef(Talent model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            this.Id = model.Id;
            this.EnglishName = model.EnglishName;
            this.ChineseName = model.ChineseName;
            this.Contact = model.Contact;
        }

        public static implicit operator TalentRef(Talent model) {
            if (model == null)
                return null;

            return new TalentRef(model);
        }
    }
}
