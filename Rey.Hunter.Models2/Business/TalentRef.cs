using System;

namespace Rey.Hunter.Models2.Business {
    public class TalentRef : ModelRef<Talent> {
        public string EnglishName { get; set; }
        public string ChineseName { get; set; }
        public TalentContact Contact { get; set; }

        public TalentRef(Talent model)
            : base(model) {
        }

        public override void Init(Talent model) {
            base.Init(model);
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
