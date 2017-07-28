using System.Collections.Generic;

namespace Rey.Hunter.Models2 {
    public abstract class AccountNodeModel : AccountModel, INodeModel {
        public string Name { get; set; }
        public bool Root { get; set; }
        public List<string> Children { get; set; } = new List<string>();
    }
}
