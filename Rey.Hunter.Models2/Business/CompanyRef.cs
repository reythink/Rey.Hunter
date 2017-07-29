using System;

namespace Rey.Hunter.Models2.Business {
    public class CompanyRef : ModelRef {
        public string Name { get; set; }

        public CompanyRef(Company model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            this.Id = model.Id;
            this.Name = model.Name;
        }

        public static implicit operator CompanyRef(Company model) {
            if (model == null)
                return null;

            return new CompanyRef(model);
        }
    }
}
