using Rey.Mon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Hunter.Models {
    public class NodeModel<TModel> : Model, IMonNodeModel<TModel, string>
        where TModel : class, IMonNodeModel<TModel, string> {
        public List<TModel> Children { get; set; } = new List<TModel>();

        public NodeModel() {
            this.Id = GenerateId();
        }
    }
}
