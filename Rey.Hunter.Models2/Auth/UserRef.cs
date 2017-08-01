﻿using System;

namespace Rey.Hunter.Models2 {
    public class UserRef : ModelRef<User> {
        public string Name { get; set; }
        public string Email { get; set; }

        public UserRef(User model)
            : base(model) {
        }

        public override void Init(User model) {
            base.Init(model);
            this.Name = model.Name;
            this.Email = model.Email;
        }

        public static implicit operator UserRef(User model) {
            if (model == null)
                return null;

            return new UserRef(model);
        }
    }
}

