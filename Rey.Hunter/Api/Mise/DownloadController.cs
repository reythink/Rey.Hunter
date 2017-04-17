using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Hunter.Api {
    [Route("/Api/[controller]/")]
    public class DownloadController : ReyController {
        [HttpGet("{id}")]
        public Task<IActionResult> Get(string id) {
            return this.InvokeAsync(() => {
                var bucket = new GridFSBucket(this.GetMonDatabase().MongoDatabase);
                var objId = new ObjectId(id);
                var fileInfo = bucket.Find(new BsonDocument("_id", objId)).FirstOrDefault();
                var contentType = fileInfo.Metadata["ContentType"].ToString();
                var output = bucket.OpenDownloadStream(objId);
                return File(output, contentType, fileInfo.Filename);
            });
        }
    }
}
