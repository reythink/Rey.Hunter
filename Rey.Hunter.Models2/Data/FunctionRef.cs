using System;

namespace Rey.Hunter.Models2.Data {
    public class FunctionRef : ModelRef {
        public string Name { get; set; }

        public FunctionRef(Function model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            this.Id = model.Id;
            this.Name = model.Name;
        }

        public static implicit operator FunctionRef(Function model) {
            if (model == null)
                return null;

            return new FunctionRef(model);
        }
    }
}
