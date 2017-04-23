using Rey.Hunter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.AspNetCore.Mvc {
    public abstract class ReyStringModelController<TModel> : ReyModelController<TModel, string>
        where TModel : Model {
    }
}
