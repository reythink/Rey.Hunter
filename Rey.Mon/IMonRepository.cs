using Rey.Mon.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Rey.Mon {
    public interface IMonRepository<TModel, TKey> : IMonCollection<TModel>
        where TModel : class, IMonModel<TKey> {
        void Upsert(TModel model);
    }
}
