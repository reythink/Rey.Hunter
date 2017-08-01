namespace Rey.Hunter.Models2.Data {
    public class LocationRef : NodeModelRef<Location> {
        public LocationRef(Location model)
            : base(model) {
        }

        public static implicit operator LocationRef(Location model) {
            if (model == null)
                return null;

            return new LocationRef(model);
        }
    }
}
