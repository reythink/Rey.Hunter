namespace Rey.Hunter.Models2.Data {
    public class IndustryRef : NodeModelRef<Industry> {
        public IndustryRef(Industry model)
            : base(model) {
        }

        public static implicit operator IndustryRef(Industry model) {
            if (model == null)
                return null;

            return new IndustryRef(model);
        }
    }
}
