namespace Rey.Hunter.Models2.Business {
    public class CompanyRef : Model {
        public string Name { get; set; }

        public CompanyRef(Company model) {
            this.Id = model.Id;
            this.Name = model.Name;
        }

        public static implicit operator CompanyRef(Company model) {
            return new CompanyRef(model);
        }
    }
}
