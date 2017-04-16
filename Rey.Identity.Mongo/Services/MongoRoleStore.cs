using Rey.Identity.Models;
using Rey.Mon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Identity.Services {
    public class MongoRoleStore<TRole> : IRoleStore<TRole>
        where TRole : class, IRole {
        private IMonDatabase Database { get; }
        public MongoRoleStore(IMonDatabase database) {
            this.Database = database;
        }

        public void InsertOne(TRole role) {
            this.Database.GetCollection<TRole>().InsertOne(role);
        }
    }
}
