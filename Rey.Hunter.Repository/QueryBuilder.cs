namespace Rey.Hunter.Repository {
    public class QueryBuilder<TModel> : IQueryBuilder<TModel> {
        private IRepository<TModel> Repository { get; }

        public QueryBuilder(IRepository<TModel> repository) {
            this.Repository = repository;
        }
    }
}
