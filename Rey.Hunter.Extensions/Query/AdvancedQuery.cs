using MongoDB.Driver;
using Rey.Hunter.Models;
using Rey.Mon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rey.Hunter.Query {
    public class AdvancedQuery<TModel>
        where TModel : AccountModel {
        protected IMonDatabase DB { get; }
        protected IMonCollection<TModel> Collection {
            get { return this.DB.GetCollection<TModel>(); }
        }

        public IQueryable<TModel> Query { get; protected set; }

        public AdvancedQuery(IMonDatabase db, string accountId) {
            this.DB = db;
            this.Query = db.GetCollection<TModel>().MongoCollection
                .Find(x => x.Account.Id.Equals(accountId))
                .ToEnumerable()
                .AsQueryable();
        }
    }
}
