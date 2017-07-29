namespace Rey.Hunter.Models2 {
    public abstract class AccountNodeModel : AccountModel, INodeModel {
        public string Name { get; set; }
        public NodeModelRef Parent { get; set; }
    }
}
