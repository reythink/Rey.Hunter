using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Mon.Models {
    public class MonStringModelRef<TModel> : MonModelRef<TModel, string>
        where TModel : MonStringModel {
        public MonStringModelRef()
            : base() {
        }

        public MonStringModelRef(TModel model)
            : base(model) {
        }

        public static implicit operator MonStringModelRef<TModel>(TModel model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.IsIdEmpty())
                throw new InvalidOperationException("Id is empty!");

            return new MonStringModelRef<TModel>(model);
        }
    }
}
