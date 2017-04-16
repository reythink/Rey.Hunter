using Rey.Mon;
using Rey.Mon.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc {
    public abstract class ReyModelController<TModel, TKey> : ReyController
        where TModel : class, IMonModel<TKey> {
        protected IMonCollection<TModel> Collection {
            get { return this.GetMonCollection<TModel>(); }
        }

        protected List<Func<IQueryable<TModel>, IQueryable<TModel>>> QueryChain { get; } = new List<Func<IQueryable<TModel>, IQueryable<TModel>>>();
        protected Func<IQueryable<TModel>, IQueryable<TModel>> Query {
            set { this.QueryChain.Add(value); }
        }

        protected List<Func<IQueryable<TModel>, string, IQueryable<TModel>>> SearchQueryChain { get; } = new List<Func<IQueryable<TModel>, string, IQueryable<TModel>>>();
        protected Func<IQueryable<TModel>, string, IQueryable<TModel>> SearchQuery {
            set { this.SearchQueryChain.Add(value); }
        }

        protected List<Func<TKey, TModel>> GetChain { get; } = new List<Func<TKey, TModel>>();
        protected Func<TKey, TModel> Get {
            set { this.GetChain.Add(value); }
        }

        protected List<Action<TModel>> CreateChain { get; } = new List<Action<TModel>>();
        protected Action<TModel> Create {
            set { this.CreateChain.Add(value); }
        }

        protected List<Func<TModel, UpdateDefinitionBuilder<TModel>, UpdateDefinition<TModel>>> UpdateChain { get; } = new List<Func<TModel, UpdateDefinitionBuilder<TModel>, UpdateDefinition<TModel>>>();
        protected Func<TModel, UpdateDefinitionBuilder<TModel>, UpdateDefinition<TModel>> Update {
            set { this.UpdateChain.Add(value); }
        }

        protected List<Action<TModel>> DeleteChain { get; } = new List<Action<TModel>>();
        protected Action<TModel> Delete {
            set { this.DeleteChain.Add(value); }
        }

        protected List<Action<List<TKey>>> BatchDeleteChain { get; } = new List<Action<List<TKey>>>();
        protected Action<List<TKey>> BatchDelete {
            set { this.BatchDeleteChain.Add(value); }
        }

        [HttpGet]
        public Task<IActionResult> QueryAction(string search) {
            return this.JsonInvokeManyAsync(() => {
                IQueryable<TModel> query = this.Collection.Query();
                foreach (var action in this.QueryChain) {
                    query = action?.Invoke(query);
                }
                foreach (var action in this.SearchQueryChain) {
                    query = action?.Invoke(query, search);
                }
                return query;
            });
        }

        [HttpGet("{id}")]
        public virtual Task<IActionResult> GetAction(TKey id) {
            return this.JsonInvokeOneAsync(() => {
                TModel model = null;
                foreach (var action in this.GetChain) {
                    model = action?.Invoke(id);
                }

                if (model == null)
                    model = this.Collection.FindOne(x => x.Id.Equals(id));

                return model;
            });
        }

        [HttpPost]
        public virtual Task<IActionResult> CreateAction([FromBody]TModel model) {
            return this.JsonInvokeOneAsync(() => {
                if (model == null)
                    throw new ArgumentNullException("model is null");

                foreach (var action in this.CreateChain) {
                    action?.Invoke(model);
                }
                this.Collection.InsertOne(model);
                return model;
            });
        }

        [HttpPost("{id}")]
        [HttpPut("{id}")]
        public Task<IActionResult> UpdateAction(TKey id, [FromBody]TModel model) {
            return this.JsonInvokeOneAsync(() => {
                if (model == null)
                    throw new ArgumentNullException("model is null");

                var defs = new List<UpdateDefinition<TModel>>();
                foreach (var action in this.UpdateChain) {
                    defs.Add(action.Invoke(model, Builders<TModel>.Update));
                }

                if (defs.Count == 0)
                    return this.Collection.FindOne(x => x.Id.Equals(id));

                var update = defs.FirstOrDefault();
                if (defs.Count > 1) {
                    update = Builders<TModel>.Update.Combine(defs);
                }

                this.Collection.MongoCollection.UpdateOne(x => x.Id.Equals(id), update);
                return this.Collection.FindOne(x => x.Id.Equals(id));
            });
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> DeleteAction(TKey id) {
            return this.JsonInvokeAsync(() => {
                var model = this.Collection.FindOne(x => x.Id.Equals(id));
                if (model == null)
                    throw new InvalidOperationException("Invalid Id");

                foreach (var action in this.DeleteChain) {
                    action?.Invoke(model);
                }

                this.Collection.DeleteOne(x => x.Id.Equals(id));
            });
        }

        [HttpDelete]
        public Task<IActionResult> DeleteAction([FromBody]TKey[] id_list) {
            return this.JsonInvokeAsync(() => {
                if (id_list == null)
                    throw new InvalidOperationException("Id list is null");

                if (id_list.Length == 0)
                    return;

                var list = id_list.ToList();
                foreach (var action in this.BatchDeleteChain) {
                    action?.Invoke(list);
                }

                this.Collection.DeleteMany(x => list.Contains(x.Id));
            });
        }
    }
}
