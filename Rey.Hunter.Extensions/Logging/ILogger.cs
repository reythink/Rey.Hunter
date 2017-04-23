using Rey.Mon.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Rey.Hunter.ModelLogging {
    public interface ILogger {
        ILogger Log<TModel, TKey>(Log<TModel, TKey> log)
            where TModel : class, IMonModel<TKey>;

        IEnumerable<Log<TModel, TKey>> Logs<TModel, TKey>(Expression<Func<Log<TModel, TKey>, bool>> filter)
            where TModel : class, IMonModel<TKey>;
    }
}
