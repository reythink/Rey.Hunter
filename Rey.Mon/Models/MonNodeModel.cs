using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Mon.Models {
    public class MonNodeModel<TModel> : MonModel, IMonNodeModel<TModel, ObjectId>
        where TModel : class, IMonNodeModel<TModel, ObjectId> {
        public List<TModel> Children { get; set; } = new List<TModel>();
    }

    public class MonStringNodeModel<TModel> : MonStringModel, IMonNodeModel<TModel, string>
        where TModel : class, IMonNodeModel<TModel, string> {
        public List<TModel> Children { get; set; } = new List<TModel>();
    }

    public class MonGuidNodeModel<TModel> : MonGuidModel, IMonNodeModel<TModel, Guid>
        where TModel : class, IMonNodeModel<TModel, Guid> {
        public List<TModel> Children { get; set; } = new List<TModel>();
    }
}
