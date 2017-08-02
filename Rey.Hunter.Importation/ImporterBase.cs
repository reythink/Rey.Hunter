using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Attributes;
using System.Collections.Generic;
using System.Reflection;

namespace Rey.Hunter.Importation {
    public abstract class ImporterBase {
        protected IImportManager Manager { get; }
        public ImporterBase(IImportManager manager) {
            this.Manager = manager;
        }
    }

    public abstract class ImporterBase<TModel> : ImporterBase
        where TModel : class, IModel, new() {
        public ImporterBase(IImportManager manager)
            : base(manager) {
        }

        protected virtual ImportTool<TModel> BeginImport() {
            return new ImportTool<TModel>(this.Manager);
        }

        protected virtual ImportTool<TModel> BeginImport(List<TModel> models) {
            return new ImportTool<TModel>(this.Manager, models);
        }
    }
}