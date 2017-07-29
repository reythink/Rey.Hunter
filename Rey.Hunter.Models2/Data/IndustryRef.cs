using System;
using System.Collections.Generic;

namespace Rey.Hunter.Models2.Data {
    public class IndustryRef : ModelRef {
        public string Name { get; set; }

        public IndustryRef(Industry model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            this.Id = model.Id;
            this.Name = model.Name;
        }

        public static implicit operator IndustryRef(Industry model) {
            if (model == null)
                return null;

            return new IndustryRef(model);
        }
    }
}
