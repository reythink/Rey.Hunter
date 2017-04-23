using Rey.Mon;
using Rey.Mon.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Rey.Hunter.ModelLogging {
    public class Logger : ILogger {
        private IMonDatabase DB { get; }

        public Logger(IMonDatabase db) {
            this.DB = db;
        }

        public ILogger Log<TModel, TKey>(Log<TModel, TKey> log)
            where TModel : class, IMonModel<TKey> {
            this.DB.GetCollection<Log<TModel, TKey>>().InsertOne(log);
            return this;
        }

        public IEnumerable<Log<TModel, TKey>> Logs<TModel, TKey>(Expression<Func<Log<TModel, TKey>, bool>> filter)
            where TModel : class, IMonModel<TKey> {
            return this.DB.GetCollection<Log<TModel, TKey>>().FindMany(filter);
        }
    }
}
