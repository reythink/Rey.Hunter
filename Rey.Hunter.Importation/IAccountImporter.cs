using Rey.Hunter.Models2;
using System.Collections.Generic;

namespace Rey.Hunter.Importation {
    public interface IAccountImporter {
        void Import(Account account);
    }

    public interface IAccountImporter<TModel> : IAccountImporter {
    }
}