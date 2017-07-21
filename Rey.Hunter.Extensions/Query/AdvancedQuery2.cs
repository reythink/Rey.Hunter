using MongoDB.Bson;
using MongoDB.Driver;
using Rey.Hunter.Models.Business;
using Rey.Mon;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Rey.Mon.Attributes;

namespace Rey.Hunter.Query {
    public abstract class AdvancedQuery2 {
        private List<FilterDefinition<BsonDocument>> Filters { get; } = new List<FilterDefinition<BsonDocument>>();

        protected IMonDatabase DB { get; }
        protected IMongoDatabase MongoDB => this.DB.MongoDatabase;
        protected IMongoCollection<BsonDocument> MongoCollection => this.MongoDB.GetCollection<BsonDocument>(this.GetCollectionName());
        protected FilterDefinitionBuilder<BsonDocument> Filter => Builders<BsonDocument>.Filter;
        protected SortDefinitionBuilder<BsonDocument> Sort => Builders<BsonDocument>.Sort;

        public AdvancedQuery2(IMonDatabase db, string accountId) {
            this.DB = db;
            this.AddFilter(this.Filter.Eq("Account._id", accountId));
        }

        protected abstract string GetCollectionName();
        public abstract IEnumerable<string> Query();

        protected virtual void AddFilter(FilterDefinition<BsonDocument> filter) {
            this.Filters.Add(filter);
        }

        protected virtual FilterDefinition<BsonDocument> Build() {
            if (this.Filters.Count == 0)
                return this.Filter.Empty;

            return this.Filter.And(this.Filters);
        }
    }

    public class TalentAdvancedQuery2 : AdvancedQuery2 {
        public TalentAdvancedQuery2(IMonDatabase db, string accountId)
            : base(db, accountId) {
        }

        protected override string GetCollectionName() {
            return MonCollectionAttribute.GetName<Talent>();
        }

        public override IEnumerable<string> Query() {
            return this.MongoCollection.Aggregate()
                .Unwind("Experiences", new AggregateUnwindOptions<Talent>() { IncludeArrayIndex = "Experiences.Index" })
                .Lookup("bus.companies", "Experiences.Company._id", "_id", "Experiences.Company")
                .Match(this.Build())
                .Group("{_id: '$_id'}")
                .ToEnumerable()
                .Select(x => x["_id"].AsString);
        }

        public IEnumerable<Talent> QueryModels() {
            var collection = this.DB.GetCollection<Talent>().MongoCollection;
            var list = this.Query();
            return collection.Find(Builders<Talent>.Filter.In(x => x.Id, list)).ToEnumerable();
        }

        public TalentAdvancedQuery2 Search(string value) {
            if (string.IsNullOrEmpty(value))
                return this;

            var filter = this.Filter.Or(
                this.Filter.Regex("EnglishName", new BsonRegularExpression(value, "ig"))
                , this.Filter.Regex("ChineseName", new BsonRegularExpression(value, "ig"))
                , this.Filter.Regex("Mobile", new BsonRegularExpression(value, "ig"))
                , this.Filter.Regex("Phone", new BsonRegularExpression(value, "ig"))
                , this.Filter.Regex("QQ", new BsonRegularExpression(value, "ig"))
                , this.Filter.Regex("Email", new BsonRegularExpression(value, "ig"))
                , this.Filter.Regex("Wechat", new BsonRegularExpression(value, "ig"))
                , this.Filter.Regex("Experiences.Company.Name", new BsonRegularExpression(value, "ig"))
                );
            AddFilter(filter);

            return this;
        }

        public TalentAdvancedQuery2 Company(params string[] values) {
            if (values == null || values.Length == 0)
                return this;

            var filter = this.Filter.Or(
                values.Select(
                    value => this.Filter.And(this.Filter.Regex("Experiences.Company.Name", new BsonRegularExpression(value, "i")), this.Filter.Eq("Experiences.CurrentJob", true))
                    )
                );
            AddFilter(filter);

            return this;
        }

        public TalentAdvancedQuery2 PreviousCompany(params string[] values) {
            if (values == null || values.Length == 0)
                return this;

            var filter = this.Filter.Or(
                values.Select(
                    value => this.Filter.And(this.Filter.Regex("Experiences.Company.Name", new BsonRegularExpression(value, "i")), this.Filter.Ne("Experiences.CurrentJob", true))
                    )
                );
            AddFilter(filter);

            return this;
        }

        public TalentAdvancedQuery2 Title(params string[] values) {
            if (values == null || values.Length == 0)
                return this;

            var filter = this.Filter.Or(
                values.Select(
                    value => this.Filter.And(this.Filter.Regex("Experiences.Title", new BsonRegularExpression(value, "i")), this.Filter.Eq("Experiences.CurrentJob", true))
                    )
                );
            AddFilter(filter);

            return this;
        }

        public TalentAdvancedQuery2 InChargeOf(params string[] values) {
            if (values == null || values.Length == 0)
                return this;

            var filter = this.Filter.Or(
                 values.Select(
                     value => this.Filter.And(this.Filter.Regex("Experiences.Responsibility", new BsonRegularExpression(value, "i")), this.Filter.Eq("Experiences.CurrentJob", true))
                     )
                 );
            AddFilter(filter);

            return this;
        }

        public TalentAdvancedQuery2 Grade(params string[] values) {
            if (values == null || values.Length == 0)
                return this;

            var filter = this.Filter.Or(
                 values.Select(
                     value => this.Filter.And(this.Filter.Regex("Experiences.Grade", new BsonRegularExpression(value, "i")), this.Filter.Eq("Experiences.CurrentJob", true))
                     )
                 );
            AddFilter(filter);

            return this;
        }

        public TalentAdvancedQuery2 Industry(params string[] values) {
            if (values == null || values.Length == 0)
                return this;

            var filter = this.Filter.AnyIn("Industries._id", values);
            AddFilter(filter);

            return this;
        }

        public TalentAdvancedQuery2 CrossIndustry(params string[] values) {
            if (values == null || values.Length == 0)
                return this;

            var filter = this.Filter.AnyIn("ProfileLabel.CrossIndustries._id", values);
            AddFilter(filter);

            return this;
        }

        public TalentAdvancedQuery2 Function(params string[] values) {
            if (values == null || values.Length == 0)
                return this;

            var filter = this.Filter.AnyIn("Functions._id", values);
            AddFilter(filter);

            return this;
        }

        public TalentAdvancedQuery2 CrossFunction(params string[] values) {
            if (values == null || values.Length == 0)
                return this;

            var filter = this.Filter.AnyIn("ProfileLabel.CrossFunctions._id", values);
            AddFilter(filter);

            return this;
        }

        public TalentAdvancedQuery2 CrossCategory(params string[] values) {
            if (values == null || values.Length == 0)
                return this;

            var filter = this.Filter.AnyIn("ProfileLabel.CrossCategories._id", values);
            AddFilter(filter);

            return this;
        }

        public TalentAdvancedQuery2 CrossChannel(params string[] values) {
            if (values == null || values.Length == 0)
                return this;

            var filter = this.Filter.AnyIn("ProfileLabel.CrossChannels._id", values);
            AddFilter(filter);

            return this;
        }

        public TalentAdvancedQuery2 BrandsHadManaged(params string[] values) {
            if (values == null || values.Length == 0)
                return this;

            var filter = this.Filter.Or(
                values.Select(
                    value => this.Filter.Regex("ProfileLabel.BrandExp", new BsonRegularExpression(value, "i"))
                    )
                );
            AddFilter(filter);

            return this;
        }

        public TalentAdvancedQuery2 KAHadManaged(params string[] values) {
            if (values == null || values.Length == 0)
                return this;

            var filter = this.Filter.Or(
                values.Select(
                    value => this.Filter.Regex("ProfileLabel.KeyAccountExp", new BsonRegularExpression(value, "i"))
                    )
                );
            AddFilter(filter);

            return this;
        }

        public TalentAdvancedQuery2 CurrentLocation(params string[] values) {
            if (values == null || values.Length == 0)
                return this;

            var filter = this.Filter.AnyIn("CurrentLocations._id", values);
            AddFilter(filter);

            return this;
        }

        public TalentAdvancedQuery2 MobilityLocation(params string[] values) {
            if (values == null || values.Length == 0)
                return this;

            var filter = this.Filter.AnyIn("MobilityLocations._id", values);
            AddFilter(filter);

            return this;
        }

        public TalentAdvancedQuery2 Gender(params int[] values) {
            if (values == null || values.Length == 0)
                return this;

            var filter = this.Filter.In("Gender", values);
            AddFilter(filter);

            return this;
        }

        public TalentAdvancedQuery2 Education(params int[] values) {
            if (values == null || values.Length == 0)
                return this;

            var filter = this.Filter.In("EducationLevel", values);
            AddFilter(filter);

            return this;
        }

        public TalentAdvancedQuery2 Language(params int[] values) {
            if (values == null || values.Length == 0)
                return this;

            var filter = this.Filter.In("Language", values);
            AddFilter(filter);

            return this;
        }

        public TalentAdvancedQuery2 Nationality(params int[] values) {
            if (values == null || values.Length == 0)
                return this;

            var filter = this.Filter.In("Nationality", values);
            AddFilter(filter);

            return this;
        }

        public TalentAdvancedQuery2 JobIntension(params int[] values) {
            if (values == null || values.Length == 0)
                return this;

            var filter = this.Filter.In("Intension", values);
            AddFilter(filter);

            return this;
        }

        public TalentAdvancedQuery2 CV(params string[] values) {
            if (values == null || values.Length == 0)
                return this;

            var filter = this.Filter.Or(
                values.Select(
                    value => this.Filter.Regex("CV", new BsonRegularExpression(value, "i"))
                    )
                );
            AddFilter(filter);

            return this;
        }

        public TalentAdvancedQuery2 Notes(params string[] values) {
            if (values == null || values.Length == 0)
                return this;

            var filter = this.Filter.Or(
                values.Select(
                    value => this.Filter.Regex("Notes", new BsonRegularExpression(value, "i"))
                    )
                );
            AddFilter(filter);

            return this;
        }
    }
}
