using Rey.Hunter.TagHelpers.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Rey.Hunter {
    public enum SortDirection {
        Asc,
        Desc,
    }

    public static class QueryExtensions {
        public static IQueryable<T> Page<T>(this IQueryable<T> query, int page, int size, Action<PaginationData> data = null) {
            data?.Invoke(new PaginationData(query.Count(), page, size));
            return query.Skip((page - 1) * size).Take(size);
        }

        public static IQueryable<T> Sort<T>(this IQueryable<T> query, Expression<Func<T, object>> field, SortDirection direction) {
            if (field == null)
                throw new ArgumentNullException(nameof(field));

            if (direction == SortDirection.Asc)
                return query.OrderBy(field);

            if (direction == SortDirection.Desc)
                return query.OrderByDescending(field);

            return query;
        }

        public static IQueryable<T> Sort<T>(this IQueryable<T> query, Expression<Func<T, object>> field, string direction) {
            if (field == null)
                throw new ArgumentNullException(nameof(field));

            if (direction == null)
                throw new ArgumentNullException(nameof(direction));

            return query.Sort(field, (SortDirection)Enum.Parse(typeof(SortDirection), direction, true));
        }

        public static IQueryable<T> Sort<T>(this IQueryable<T> query, string field, string direction, Action<IDictionary<string, Expression<Func<T, object>>>> config) {
            if (field == null)
                throw new ArgumentNullException(nameof(field));

            if (direction == null)
                throw new ArgumentNullException(nameof(direction));

            if (config == null)
                throw new ArgumentNullException(nameof(config));

            var dic = new Dictionary<string, Expression<Func<T, object>>>();
            config(dic);
            var pair = dic.Single(x => x.Key.Equals(field, StringComparison.CurrentCultureIgnoreCase));
            return query.Sort(pair.Value, direction);
        }
    }
}
