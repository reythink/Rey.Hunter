using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Mon {
    public interface IMonGridFSBucket {
        IMonDatabase Database { get; }
        IGridFSBucket Bucket { get; }

        GridFSFileInfo FindById(ObjectId id);
        GridFSFileInfo FindById(string id);

        IAsyncCursor<GridFSFileInfo> FindByMD5(string md5);
        IAsyncCursor<GridFSFileInfo> FindByMD5Of(byte[] buffer);
        IAsyncCursor<GridFSFileInfo> FindByMD5Of(Stream input);

        IAsyncCursor<GridFSFileInfo> FindByFileName(string fileName);
    }
}
