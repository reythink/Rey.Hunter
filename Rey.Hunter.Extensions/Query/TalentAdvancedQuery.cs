using Rey.Hunter.Models.Business;
using Rey.Mon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rey.Hunter.Query {
    public class TalentAdvancedQuery : AdvancedQuery<Talent> {
        public TalentAdvancedQuery(IMonDatabase db, string accountId)
            : base(db, accountId) {
        }

        private bool Search(Talent model, string value) {
            if (model.EnglishName != null && model.EnglishName.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) != -1)
                return true;

            if (model.ChineseName != null && model.ChineseName.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) != -1)
                return true;

            if (model.Mobile != null && model.Mobile.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) != -1)
                return true;

            if (model.Phone != null && model.Phone.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) != -1)
                return true;

            if (model.QQ != null && model.QQ.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) != -1)
                return true;

            if (model.Email != null && model.Email.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) != -1)
                return true;

            if (model.Wechat != null && model.Wechat.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) != -1)
                return true;

            if (model.Experiences != null
                && model.Experiences.Count > 0
                && model.Experiences.Any(exp => {
                    if (!exp.CurrentJob.HasValue || !exp.CurrentJob.Value) { return false; }
                    var name = exp.Company?.Concrete(this.DB)?.Name;
                    if (name == null) { return false; }
                    return name.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) != -1;
                })) {
                return true;
            }

            return false;
        }

        public TalentAdvancedQuery Search(string value) {
            if (string.IsNullOrEmpty(value))
                return this;

            this.Query = this.Query.Where(
                model => Search(model, value)
                );

            return this;
        }

        private bool Title(Talent model, string value) {
            var experience = model.Experiences.Find(x => x.CurrentJob == true);
            if (experience == null || experience.Title == null)
                return false;

            return experience.Title.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) != -1;
        }

        public TalentAdvancedQuery Title(string[] values) {
            if (values == null || values.Length == 0)
                return this;

            this.Query = this.Query.Where(
                model => values.Any(
                    value => Title(model, value)
                    )
                );
            return this;
        }

        private bool InChargeOf(Talent model, string value) {
            var experience = model.Experiences.Find(x => x.CurrentJob == true);
            if (experience == null || experience.Responsibility == null)
                return false;

            return experience.Responsibility.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) != -1;
        }

        public TalentAdvancedQuery InChargeOf(string[] values) {
            if (values == null || values.Length == 0)
                return this;

            this.Query = this.Query.Where(
                model => values.Any(
                    value => InChargeOf(model, value)
                    )
                );
            return this;
        }

        private bool Grade(Talent model, string value) {
            var experience = model.Experiences.Find(x => x.CurrentJob == true);
            if (experience == null || experience.Grade == null)
                return false;

            return experience.Grade.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) != -1;
        }

        public TalentAdvancedQuery Grade(string[] values) {
            if (values == null || values.Length == 0)
                return this;

            this.Query = this.Query.Where(
                model => values.Any(
                    value => Grade(model, value)
                    )
                );
            return this;
        }

        public TalentAdvancedQuery Industry(string[] values) {
            if (values == null || values.Length == 0)
                return this;

            this.Query = this.Query.Where(
                model => model.Industries.Any(
                    industry => values.Contains(industry.Id)
                    )
                );
            return this;
        }

        public TalentAdvancedQuery CrossIndustry(string[] values) {
            if (values == null || values.Length == 0)
                return this;

            this.Query = this.Query.Where(
                model => model.ProfileLabel.CrossIndustries.Any(
                    industry => values.Contains(industry.Id)
                    )
                );
            return this;
        }

        public TalentAdvancedQuery Function(string[] values) {
            if (values == null || values.Length == 0)
                return this;

            this.Query = this.Query.Where(
                model => model.Functions.Any(
                    function => values.Contains(function.Id)
                    )
                );
            return this;
        }

        public TalentAdvancedQuery CrossFunction(string[] values) {
            if (values == null || values.Length == 0)
                return this;

            this.Query = this.Query.Where(
                model => model.ProfileLabel.CrossFunctions.Any(
                    function => values.Contains(function.Id)
                    )
                );
            return this;
        }

        public TalentAdvancedQuery CrossCategory(string[] values) {
            if (values == null || values.Length == 0)
                return this;

            this.Query = this.Query.Where(
                model => model.ProfileLabel.CrossCategories.Any(
                    category => values.Contains(category.Id)
                    )
                );
            return this;
        }

        public TalentAdvancedQuery CrossChannel(string[] values) {
            if (values == null || values.Length == 0)
                return this;

            this.Query = this.Query.Where(
                model => model.ProfileLabel.CrossChannels.Any(
                    channel => values.Contains(channel.Id)
                    )
                );
            return this;
        }

        private bool BrandsHadManaged(Talent model, string value) {
            if (model.ProfileLabel == null || model.ProfileLabel.BrandExp == null)
                return false;

            return model.ProfileLabel.BrandExp.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) != -1;
        }

        public TalentAdvancedQuery BrandsHadManaged(string[] values) {
            if (values == null || values.Length == 0)
                return this;

            this.Query = this.Query.Where(
                model => values.Any(
                    value => BrandsHadManaged(model, value)
                    )
                );
            return this;
        }

        private bool KAHadManaged(Talent model, string value) {
            if (model.ProfileLabel == null || model.ProfileLabel.KeyAccountExp == null)
                return false;

            return model.ProfileLabel.KeyAccountExp.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) != -1;
        }

        public TalentAdvancedQuery KAHadManaged(string[] values) {
            if (values == null || values.Length == 0)
                return this;

            this.Query = this.Query.Where(
                model => values.Any(
                    value => KAHadManaged(model, value)
                    )
                );
            return this;
        }

        public TalentAdvancedQuery CurrentLocation(string[] values) {
            if (values == null || values.Length == 0)
                return this;

            this.Query = this.Query.Where(
                model => model.CurrentLocations.Any(
                    location => values.Contains(location.Id)
                    )
                );
            return this;
        }

        public TalentAdvancedQuery MobilityLocation(string[] values) {
            if (values == null || values.Length == 0)
                return this;

            this.Query = this.Query.Where(
                model => model.MobilityLocations.Any(
                    location => values.Contains(location.Id)
                    )
                );
            return this;
        }

        public TalentAdvancedQuery Gender(string[] values) {
            if (values == null || values.Length == 0)
                return this;

            this.Query = this.Query.Where(
                model => values.Select(value => (Gender?)Enum.Parse(typeof(Gender), value)).Contains(model.Gender)
                );
            return this;
        }

        public TalentAdvancedQuery Education(string[] values) {
            if (values == null || values.Length == 0)
                return this;

            this.Query = this.Query.Where(
                model => values.Select(value => (EducationLevel?)Enum.Parse(typeof(EducationLevel), value)).Contains(model.EducationLevel)
                );
            return this;
        }

        public TalentAdvancedQuery Language(string[] values) {
            if (values == null || values.Length == 0)
                return this;

            this.Query = this.Query.Where(
                model => values.Select(value => (Language?)Enum.Parse(typeof(Language), value)).Contains(model.Language)
                );
            return this;
        }

        public TalentAdvancedQuery Nationality(string[] values) {
            if (values == null || values.Length == 0)
                return this;

            this.Query = this.Query.Where(
                model => values.Select(value => (Nationality?)Enum.Parse(typeof(Nationality), value)).Contains(model.Nationality)
                );
            return this;
        }

        public TalentAdvancedQuery JobIntension(string[] values) {
            if (values == null || values.Length == 0)
                return this;

            this.Query = this.Query.Where(
                model => values.Select(value => (JobIntension?)Enum.Parse(typeof(JobIntension), value)).Contains(model.Intension)
                );
            return this;
        }

        private bool CV(Talent model, string value) {
            if (model.CV == null)
                return false;

            return model.CV.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) != -1;
        }

        public TalentAdvancedQuery CV(string[] values) {
            if (values == null || values.Length == 0)
                return this;

            this.Query = this.Query.Where(
                model => values.Any(
                    value => CV(model, value)
                    )
                );
            return this;
        }

        private bool Notes(Talent model, string value) {
            if (model.Notes == null)
                return false;

            return model.Notes.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) != -1;
        }

        public TalentAdvancedQuery Notes(string[] values) {
            if (values == null || values.Length == 0)
                return this;

            this.Query = this.Query.Where(
                model => values.Any(
                    value => Notes(model, value)
                    )
                );
            return this;
        }
    }
}
