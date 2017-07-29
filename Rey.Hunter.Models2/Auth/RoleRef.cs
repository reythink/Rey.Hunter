using System;

namespace Rey.Hunter.Models2 {
    public class RoleRef : ModelRef {
        public string Name { get; set; }

        public RoleRef(Role model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            this.Id = model.Id;
            this.Name = model.Name;
        }

        public static implicit operator RoleRef(Role model) {
            if (model == null)
                return null;

            return new RoleRef(model);
        }
    }
}

