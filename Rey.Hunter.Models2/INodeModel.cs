using System.Collections.Generic;

namespace Rey.Hunter.Models2 {
    public interface INodeModel {
        string Name { get; }
        bool Root { get; }
        List<string> Children { get; }
    }
}
