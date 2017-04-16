using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Mon.Models {
    public interface IMonModelRef<TModel, TKey>
        where TModel : class, IMonModel<TKey> {
        TKey Id { get; set; }

        TModel Concrete(IMonCollection<TModel> collection);
        TModel Concrete(IMonDatabase database);
    }
}
