using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Mon.Models {
    public interface IMonNodeModelRef<TModel, TKey>
        where TModel : class, IMonNodeModel<TModel, TKey> {
        TKey Id { get; set; }

        TModel Concrete(IMonNodeRepository<TModel, TKey> repository);
        TModel Concrete(IMonDatabase database);
    }
}
