﻿using Rey.Mon;
using Rey.Mon.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rey.Hunter.ModelLogging;

namespace Microsoft.AspNetCore.Mvc {
    public abstract class ReyModelController<TModel, TKey> : ReyController
        where TModel : class, IMonModel<TKey> {
        public event Func<IQueryable<TModel>, IQueryable<TModel>> BeforeQuery;
        public event Func<IQueryable<TModel>, string, IQueryable<TModel>> BeforeSearch;

        public event Action<TKey> BeforeGet;
        public event Action<TModel> AfterGet;

        public event Action<TModel> BeforeCreate;
        public event Action<TModel> AfterCreate;

        public event Action<BeforeUpdateEventArgs<TModel, TKey>> BeforeUpdate;
        public event Action<TKey, TModel> AfterUpdate;

        public event Action<TModel> BeforeDelete;
        public event Action<TModel> AfterDelete;

        public event Action<IEnumerable<TKey>> BeforeBatchDelete;
        public event Action<IEnumerable<TKey>> AfterBatchDelete;

        protected IMonCollection<TModel> Collection {
            get { return this.GetMonCollection<TModel>(); }
        }

        protected IMonRepository<TModel, TKey> Repository {
            get { return this.GetMonRepository<TModel, TKey>(); }
        }

        [HttpGet]
        public Task<IActionResult> QueryAction(string search) {
            return this.JsonInvokeManyAsync(() => {
                IQueryable<TModel> query = this.Collection.Query();
                query = this.BeforeQuery?.Invoke(query) ?? query;

                if (!string.IsNullOrEmpty(search)) {
                    query = this.BeforeSearch?.Invoke(query, search) ?? query;
                }

                return query;
            });
        }

        [HttpGet("{id}")]
        public virtual Task<IActionResult> GetAction(TKey id) {
            return this.JsonInvokeOneAsync(() => {
                if (id == null)
                    throw new ArgumentNullException($"{nameof(id)} is null");

                this.BeforeGet?.Invoke(id);
                var model = this.Collection.FindOne(x => x.Id.Equals(id));
                if (model == null)
                    throw new InvalidOperationException($"Cannot find model by id. ${id}");

                this.AfterGet?.Invoke(model);
                return model;
            });
        }

        [HttpPost]
        public virtual Task<IActionResult> CreateAction([FromBody]TModel model) {
            return this.JsonInvokeOneAsync(() => {
                if (model == null)
                    throw new ArgumentNullException($"{nameof(model)} is null");

                this.BeforeCreate?.Invoke(model);
                this.Collection.InsertOne(model);
                this.AfterCreate?.Invoke(model);

                this.Log<TModel, TKey>(LogAction.Create, model);

                return model;
            });
        }

        [HttpPost("{id}")]
        [HttpPut("{id}")]
        public Task<IActionResult> UpdateAction(TKey id, [FromBody]TModel model) {
            return this.JsonInvokeOneAsync(() => {
                if (id == null)
                    throw new ArgumentNullException($"{nameof(id)} is null");

                if (model == null)
                    throw new ArgumentNullException($"{nameof(model)} is null");

                var args = new BeforeUpdateEventArgs<TModel, TKey>(id, model);
                this.BeforeUpdate?.Invoke(args);
                this.Collection.ReplaceOne(x => x.Id.Equals(id), model);
                model = this.Collection.FindOne(x => x.Id.Equals(id));
                this.AfterUpdate?.Invoke(id, model);

                this.Log<TModel, TKey>(LogAction.Update, id);

                return model;
            });
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> DeleteAction(TKey id) {
            return this.JsonInvokeAsync(() => {
                if (id == null)
                    throw new ArgumentNullException($"{nameof(id)} is null");

                var model = this.Collection.FindOne(x => x.Id.Equals(id));
                if (model == null)
                    throw new InvalidOperationException($"Cannot find model by id. ${id}");

                this.BeforeDelete?.Invoke(model);
                this.Collection.DeleteOne(x => x.Id.Equals(id));
                this.AfterDelete?.Invoke(model);

                this.Log<TModel, TKey>(LogAction.Delete, id);
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
                this.BeforeBatchDelete?.Invoke(list);
                this.Collection.DeleteMany(x => list.Contains(x.Id));
                this.AfterBatchDelete?.Invoke(list);

                list.ForEach(id => this.Log<TModel, TKey>(LogAction.Delete, id));
            });
        }
    }
}
