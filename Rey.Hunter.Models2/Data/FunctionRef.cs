namespace Rey.Hunter.Models2.Data {
    public class FunctionRef : NodeModelRef {
        public FunctionRef(Function model)
            : base(model) {
        }

        public static implicit operator FunctionRef(Function model) {
            if (model == null)
                return null;

            return new FunctionRef(model);
        }
    }
}
