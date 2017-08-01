using System;

namespace Rey.Hunter.Models2 {
    public class RoleRef : ModelRef<Role> {
        public string Name { get; set; }

        public RoleRef(Role model)
            : base(model) {
        }

        public override void Init(Role model) {
            base.Init(model);
            this.Name = model.Name;
        }

        public static implicit operator RoleRef(Role model) {
            if (model == null)
                return null;

            return new RoleRef(model);
        }
    }
}

