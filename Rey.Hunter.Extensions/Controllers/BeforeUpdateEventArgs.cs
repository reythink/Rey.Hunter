using Rey.Mon.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Microsoft.AspNetCore.Mvc {
    public class BeforeUpdateEventArgs<TModel, TKey> : EventArgs
       where TModel : class, IMonModel<TKey> {
        public TKey Id { get; }
        public TModel Model { get; }
        public List<Expression<Func<TModel, object>>> Ignores { get; } = new List<Expression<Func<TModel, object>>>();

        public BeforeUpdateEventArgs(TKey id, TModel model) {
            this.Id = id;
            this.Model = model;
        }

        public BeforeUpdateEventArgs<TModel, TKey> Ignore(Expression<Func<TModel, object>> ignore) {
            this.Ignores.Add(ignore);
            return this;
        }
    }
}
