using MongoDB.Driver;
using Rey.Hunter.Models2;
using MongoDB.Driver.GridFS;

namespace Rey.Hunter.Importation {
    public class FileImporter : ImporterBase, IAccountImporter {
        public FileImporter(IImportManager manager)
            : base(manager) {
        }

        public void Import(Account account) {
            using (var tool = this.BeginImport()) {
                var impBucket = new GridFSBucket(tool.ImportDB);
                var expBucket = new GridFSBucket(tool.ExportDB);

                var filter = Builders<GridFSFileInfo>.Filter.Empty;
                var files = impBucket.Find(filter).ToEnumerable();
                expBucket.Drop();
                foreach (var file in files) {
                    var buffer = impBucket.DownloadAsBytes(file.Id);
                    var options = new GridFSUploadOptions {
                        Metadata = file.Metadata
                    };
                    options.Metadata.Add("UploadDate", file.UploadDateTime);
                    expBucket.UploadFromBytes(file.Id, file.Filename, buffer, options);
                }
            }
        }
    }
}