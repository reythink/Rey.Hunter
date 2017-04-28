using Rey.Hunter.Models.Business;
using System;
using System.Collections.Generic;
using System.Text;
using Rey.Mon;
using System.Linq;

namespace Rey.Hunter.Query {
    public class CompanyAdvancedQuery : AdvancedQuery<Company> {
        public CompanyAdvancedQuery(IMonDatabase db, string accountId)
            : base(db, accountId) {
        }

        private bool Search(Company model, string value) {
            if (model.Name != null && model.Name.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) != -1)
                return true;

            return false;
        }

        public CompanyAdvancedQuery Search(string value) {
            if (string.IsNullOrEmpty(value))
                return this;

            this.Query = this.Query.Where(
                model => Search(model, value)
                );

            return this;
        }

        private bool Name(Company model, string value) {
            if (model.Name == null)
                return false;

            return model.Name.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) != -1;
        }

        public CompanyAdvancedQuery Name(string[] values) {
            if (values == null || values.Length == 0)
                return this;

            this.Query = this.Query.Where(
                model => values.Any(
                    value => Name(model, value)
                    )
                );
            return this;
        }

        public CompanyAdvancedQuery Industry(string[] values) {
            if (values == null || values.Length == 0)
                return this;

            this.Query = this.Query.Where(
                model => model.Industries.Any(
                    industry => values.Contains(industry.Id)
                    )
                );
            return this;
        }

        public CompanyAdvancedQuery Type(string[] values) {
            if (values == null || values.Length == 0)
                return this;

            this.Query = this.Query.Where(
                model => values.Select(value => (CompanyType?)Enum.Parse(typeof(CompanyType), value)).Contains(model.Type)
                );
            return this;
        }

        public CompanyAdvancedQuery Status(string[] values) {
            if (values == null || values.Length == 0)
                return this;

            this.Query = this.Query.Where(
                model => values.Select(value => (CompanyStatus?)Enum.Parse(typeof(CompanyStatus), value)).Contains(model.Status)
                );
            return this;
        }
    }
}
