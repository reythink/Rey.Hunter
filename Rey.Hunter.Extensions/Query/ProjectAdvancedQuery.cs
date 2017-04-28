using Rey.Hunter.Models.Business;
using System;
using System.Collections.Generic;
using System.Text;
using Rey.Mon;
using System.Linq;
using Rey.Hunter.Models.Identity;

namespace Rey.Hunter.Query {
    public class ProjectAdvancedQuery : AdvancedQuery<Project> {
        public ProjectAdvancedQuery(IMonDatabase db, string accountId)
            : base(db, accountId) {
        }

        private bool Search(Project model, string value) {
            if (model.Name != null && model.Name.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) != -1)
                return true;

            return false;
        }

        public ProjectAdvancedQuery Search(string value) {
            if (string.IsNullOrEmpty(value))
                return this;

            this.Query = this.Query.Where(
                model => Search(model, value)
                );

            return this;
        }

        private bool Name(Project model, string value) {
            if (model.Name == null)
                return false;

            return model.Name.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) != -1;
        }

        public ProjectAdvancedQuery Name(string[] values) {
            if (values == null || values.Length == 0)
                return this;

            this.Query = this.Query.Where(
                model => values.Any(
                    value => Name(model, value)
                    )
                );
            return this;
        }

        private bool Client(Project model, string value) {
            var name = model.Client?.Concrete(this.DB)?.Name;
            if (name == null)
                return false;

            return name.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) != -1;
        }

        public ProjectAdvancedQuery Client(string[] values) {
            if (values == null || values.Length == 0)
                return this;

            this.Query = this.Query.Where(
                model => values.Any(
                    value => Client(model, value)
                    )
                );
            return this;
        }

        public ProjectAdvancedQuery Function(string[] values) {
            if (values == null || values.Length == 0)
                return this;

            this.Query = this.Query.Where(
                model => model.Functions.Any(
                    function => values.Contains(function.Id)
                    )
                );
            return this;
        }

        private bool Manager(Project model, string value) {
            var name = model.Manager?.Concrete(this.DB)?.Name;
            if (name == null)
                return false;

            return name.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) != -1;
        }

        public ProjectAdvancedQuery Manager(string[] values) {
            if (values == null || values.Length == 0)
                return this;

            this.Query = this.Query.Where(
                model => values.Any(
                    value => Manager(model, value)
                    )
                );
            return this;
        }

        private bool Consultant(User user, string value) {
            var name = user?.Name;
            if (name == null)
                return false;

            return name.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) != -1;
        }

        public ProjectAdvancedQuery Consultant(string[] values) {
            if (values == null || values.Length == 0)
                return this;

            this.Query = this.Query.Where(
                model => model.Consultants.Select(x => x.Concrete(this.DB)).Any(
                    user => values.Any(
                        value => Consultant(user, value)
                        )
                    )
                );
            return this;
        }
    }
}
