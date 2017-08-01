using System;

namespace Rey.Hunter.Models2.Business {
    public class CompanyRef : ModelRef<Company> {
        public string Name { get; set; }

        public CompanyRef(Company model)
            : base(model) {
        }

        public override void Init(Company model) {
            base.Init(model);
            this.Name = model.Name;
        }

        public static implicit operator CompanyRef(Company model) {
            if (model == null)
                return null;

            return new CompanyRef(model);
        }
    }
}
