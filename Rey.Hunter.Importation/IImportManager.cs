using MongoDB.Driver;

namespace Rey.Hunter.Importation {
    public interface IImportManager {
        IMongoClient ImportClient { get; }
        IMongoClient ExportClient { get; }
    }
}