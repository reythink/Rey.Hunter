using Rey.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Rey.Mon;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Rey.Identity.Services {
    public class MongoUserStore<TUser> : IUserStore<TUser>
        where TUser : class, IUser {
        private IMonDatabase Database { get; }
        public MongoUserStore(IMonDatabase database) {
            this.Database = database;
        }

        public TUser GetUserByClaims(IEnumerable<Claim> claims) {
            var id = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var filter = new BsonDocument("_id", id);
            return this.Database.GetCollection<TUser>().MongoCollection.Find(filter).SingleOrDefault();
        }

        public void InsertOne(TUser user) {
            this.Database.GetCollection<TUser>().InsertOne(user);
        }
    }
}
