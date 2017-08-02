using System.Collections.Generic;

namespace Rey.Hunter.Importation {
    public interface IImporter<TModel> {
        IEnumerable<TModel> Import();
    }
}