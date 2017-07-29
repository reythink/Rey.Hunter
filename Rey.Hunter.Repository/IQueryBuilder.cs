using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Rey.Hunter.Repository {
    public interface IQueryBuilder<TModel> {
        IEnumerable<TModel> Build(Action<QueryResult> result = null);
    }

    public interface IQueryBuilder<TModel, TBuilder> : IQueryBuilder<TModel>
        where TBuilder : class, IQueryBuilder<TModel> {
        TBuilder AddFilter(string name, FilterDefinition<TModel> filter);
        TBuilder AddFilters(string name, IEnumerable<FilterDefinition<TModel>> filters);

        TBuilder SortAsc(string value);
        TBuilder SortDesc(string value);

        TBuilder Page(int index, int size = 15);
    }
}
