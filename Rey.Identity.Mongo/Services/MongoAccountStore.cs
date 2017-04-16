using Rey.Identity.Models;
using Rey.Mon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Identity.Services {
    public class MongoAccountStore<TAccount> : IAccountStore<TAccount>
        where TAccount : class, IAccount {
        private IMonDatabase Database { get; }
        public MongoAccountStore(IMonDatabase database) {
            this.Database = database;
        }

        public void InsertOne(TAccount account) {
            this.Database.GetCollection<TAccount>().InsertOne(account);
        }
    }
}
