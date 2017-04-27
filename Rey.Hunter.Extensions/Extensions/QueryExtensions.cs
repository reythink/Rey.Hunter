using Rey.Hunter.TagHelpers.Pagination;
using Rey.Mon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Rey.Hunter {
    public static class QueryExtensions {
        public static IQueryable<T> Page<T>(this IQueryable<T> query, int page, int size, Action<PaginationData> data = null) {
            data?.Invoke(new PaginationData(query.Count(), page, size));
            return query.Skip((page - 1) * size).Take(size);
        }
    }
}
