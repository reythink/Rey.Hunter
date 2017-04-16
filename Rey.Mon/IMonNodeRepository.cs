using Rey.Mon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rey.Mon {
    public interface IMonNodeRepository<TModel, TKey> : IMonRepository<TModel, TKey>
        where TModel : class, IMonNodeModel<TModel, TKey> {

        void InsertNode(TKey parentId, int index, TModel node, int generations = 10);
        void PrependNode(TKey parentId, TModel node, int generations = 10);
        void AppendNode(TKey parentId, TModel node, int generations = 10);

        void ReplaceNode(TKey id, TModel node, int generations = 10);

        void DeleteNode(TKey id, int generations = 10);

        TModel FindRootNode(TKey id, int generations = 10);
        TModel FindNode(TKey id, int generations = 10);
        TModel FindParentNode(TKey id, int generations = 10);
    }
}
