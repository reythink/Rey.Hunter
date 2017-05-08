using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.IO;
using System.Security.Cryptography;

namespace Rey.Mon {
    public class MonGridFSBucket : IMonGridFSBucket {
        public IMonDatabase Database { get; }
        public IGridFSBucket Bucket { get; }

        public MonGridFSBucket(IMonDatabase database, IGridFSBucket bucket) {
            this.Database = database;
            this.Bucket = bucket;
        }

        public GridFSFileInfo FindById(ObjectId id) {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var filter = new BsonDocument("_id", id);
            return this.Bucket.Find(filter).FirstOrDefault();
        }

        public GridFSFileInfo FindById(string id) {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return FindById(new ObjectId(id));
        }

        public IAsyncCursor<GridFSFileInfo> FindByMD5(string md5) {
            if (md5 == null)
                throw new ArgumentNullException(nameof(md5));

            var filter = new BsonDocument("md5", md5);
            return this.Bucket.Find(filter);
        }

        public IAsyncCursor<GridFSFileInfo> FindByMD5Of(byte[] buffer) {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            var md5 = string.Join("", MD5.Create().ComputeHash(buffer).Select(x => string.Format("{0:x2}", x)));
            return FindByMD5(md5);
        }

        public IAsyncCursor<GridFSFileInfo> FindByMD5Of(Stream input) {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            var md5 = string.Join("", MD5.Create().ComputeHash(input).Select(x => string.Format("{0:x2}", x)));
            return FindByMD5(md5);
        }

        public IAsyncCursor<GridFSFileInfo> FindByFileName(string fileName) {
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            var filter = new BsonDocument("filename", fileName);
            return this.Bucket.Find(filter);
        }
    }
}
