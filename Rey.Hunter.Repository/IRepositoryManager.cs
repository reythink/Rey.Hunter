using Rey.Hunter.Models2;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rey.Hunter.Repository {
    public interface IRepositoryManager {
        IAccountRepository Account();
        ICompanyRepository Company();
    }

    public interface IRepository<TModel> {

    }

    public interface IAccountRepository : IRepository<Account> {

    }

    public class AccountRepository : IAccountRepository {

    }

    public class RepositoryManager : IRepositoryManager {
        public IAccountRepository Account() {
            return new AccountRepository();
        }

        public ICompanyRepository Company() {
            throw new NotImplementedException();
        }
    }
}
