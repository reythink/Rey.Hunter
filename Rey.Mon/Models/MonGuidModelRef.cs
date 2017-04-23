using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Mon.Models {
    public class MonGuidModelRef<TModel> : MonModelRef<TModel, Guid>
        where TModel : MonGuidModel {
        public MonGuidModelRef()
            : base() {
        }

        public MonGuidModelRef(TModel model)
            : base(model) {
        }

        public MonGuidModelRef(Guid id)
            : base(id) {
        }

        public static implicit operator MonGuidModelRef<TModel>(TModel model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.IsIdEmpty())
                throw new InvalidOperationException("Id is empty!");

            return new MonGuidModelRef<TModel>(model);
        }
    }
}
