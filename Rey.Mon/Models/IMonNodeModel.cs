using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Mon.Models {
    public interface IMonNodeModel<TModel, TKey> : IMonModel<TKey>
        where TModel : class, IMonNodeModel<TModel, TKey> {
        List<TModel> Children { get; }
    }
}
