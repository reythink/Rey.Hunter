using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Rey.Hunter.Api {
    [Route("/Api/[controller]/")]
    public class UploadController : ReyController {
        [HttpPost("file")]
        public Task<IActionResult> Image() {
            return this.JsonInvokeManyAsync(() => {
                var bucket = this.GetBucket();
                var list = new List<object>();
                foreach (var formFile in this.Request.Form.Files) {
                    using (var input = formFile.OpenReadStream()) {
                        var id = bucket.FindByMD5Of(input).FirstOrDefault()?.Id;
                        if (id == null) {
                            input.Seek(0, System.IO.SeekOrigin.Begin);
                            var metadata = new BsonDocument {
                                { "ContentType", formFile.ContentType }
                            };
                            id = bucket.Bucket.UploadFromStream(formFile.FileName, input, new GridFSUploadOptions { Metadata = metadata });
                        }
                        list.Add(new { url = $"/api/download/{id}", name = formFile.FileName, contentType = formFile.ContentType });
                    }
                }

                return list;
            });
        }
    }
}
