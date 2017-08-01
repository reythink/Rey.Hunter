namespace Rey.Hunter.Models2.Data {
    public class CategoryRef : NodeModelRef<Category> {
        public CategoryRef(Category model)
            : base(model) {
        }

        public static implicit operator CategoryRef(Category model) {
            if (model == null)
                return null;

            return new CategoryRef(model);
        }
    }
}
