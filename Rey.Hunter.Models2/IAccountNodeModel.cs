namespace Rey.Hunter.Models2 {
    public interface INodeModel {
        string Name { get; }
        NodeModelRef Parent { get; }
    }
}
