using Rey.Mon.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Rey.Mon {
    public class MonRepository<TModel, TKey> : MonCollection<TModel>, IMonRepository<TModel, TKey>
        where TModel : class, IMonModel<TKey> {
        public MonRepository(IMonDatabase database, IMongoCollection<TModel> mongoCollection)
            : base(database, mongoCollection) {
        }

        public void Upsert(TModel model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.IsIdEmpty()) {
                this.InsertOne(model);
            } else {
                this.ReplaceOne(x => x.Id.Equals(model.Id), model);
            }
        }
    }
}
