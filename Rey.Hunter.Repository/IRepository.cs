namespace Rey.Hunter.Repository {
    public interface IRepository<TModel> {
        string GetDatabaseName();
        string GetCollectionName();

        void InsertOne(TModel model);
        void ReplaceOne(TModel model);
        void DeleteOne(string id);
        void Drop();
    }
}
