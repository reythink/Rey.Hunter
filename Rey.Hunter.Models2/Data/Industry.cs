using Rey.Hunter.Models2.Attributes;
using System;
using System.Text;

namespace Rey.Hunter.Models2.Data {
    [MongoCollection("data.industry")]
    public class Industry : AccountNodeModel {
    }

    public class IndustryRef : Model {
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
