using System;

namespace Rey.Hunter.Models2.Data {
    public class LocationRef : Model {
        public string Name { get; set; }

        public LocationRef(Location model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            this.Id = model.Id;
            this.Name = model.Name;
        }

        public static implicit operator LocationRef(Location model) {
            if (model == null)
                return null;

            return new LocationRef(model);
        }
    }
}
