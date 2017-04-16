using Rey.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Identity.Services {
    public interface IAccountStore<TAccount> where TAccount : class, IAccount {
        void InsertOne(TAccount account);
    }
}
