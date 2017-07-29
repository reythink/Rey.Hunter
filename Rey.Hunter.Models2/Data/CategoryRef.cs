using System;

namespace Rey.Hunter.Models2.Data {
    public class CategoryRef : ModelRef {
        public string Name { get; set; }

        public CategoryRef(Category model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            this.Id = model.Id;
            this.Name = model.Name;
        }

        public static implicit operator CategoryRef(Category model) {
            if (model == null)
                return null;

            return new CategoryRef(model);
        }
    }
}
