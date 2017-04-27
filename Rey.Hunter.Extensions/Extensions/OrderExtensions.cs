using Rey.Mon;
using Rey.Mon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Rey.Hunter {
    public class RefComparer<TModel, TKey, TProperty> : IComparer<IMonModelRef<TModel, TKey>>
        where TModel : class, IMonModel<TKey>
        where TProperty : class, IComparable {
        protected IMonDatabase DB { get; private set; }
        protected Func<TModel, TProperty> KeySelector { get; private set; }

        public RefComparer<TModel, TKey, TProperty> SetDB(IMonDatabase db) {
            this.DB = db;
            return this;
        }

        public RefComparer<TModel, TKey, TProperty> SetKeySelector(Func<TModel, TProperty> keySelector) {
            this.KeySelector = keySelector;
            return this;
        }

        public int Compare(IMonModelRef<TModel, TKey> x, IMonModelRef<TModel, TKey> y) {
            if (x == null && y == null)
                return 0;

            if (x == null)
                return -1;

            if (y == null)
                return 1;

            var item1 = x.Concrete(this.DB);
            var item2 = y.Concrete(this.DB);
            if (item1 == null && item2 == null)
                return 0;

            if (item1 == null)
                return -1;

            if (item2 == null)
                return 1;

            var key1 = this.KeySelector.Invoke(item1);
            var key2 = this.KeySelector.Invoke(item2);
            if (key1 == null && key2 == null)
                return 0;

            if (key1 == null)
                return -1;

            if (key2 == null)
                return 1;

            return key1.CompareTo(key2);
        }
    }

    public class NodeRefComparer<TModel, TKey, TProperty> : IComparer<IMonNodeModelRef<TModel, TKey>>
        where TModel : class, IMonNodeModel<TModel, TKey>
        where TProperty : class, IComparable {
        protected IMonDatabase DB { get; private set; }
        protected Func<TModel, TProperty> KeySelector { get; private set; }

        public NodeRefComparer<TModel, TKey, TProperty> SetDB(IMonDatabase db) {
            this.DB = db;
            return this;
        }

        public NodeRefComparer<TModel, TKey, TProperty> SetKeySelector(Func<TModel, TProperty> keySelector) {
            this.KeySelector = keySelector;
            return this;
        }

        public int Compare(IMonNodeModelRef<TModel, TKey> x, IMonNodeModelRef<TModel, TKey> y) {
            if (x == null && y == null)
                return 0;

            if (x == null)
                return -1;

            if (y == null)
                return 1;

            var item1 = x.Concrete(this.DB);
            var item2 = y.Concrete(this.DB);
            if (item1 == null && item2 == null)
                return 0;

            if (item1 == null)
                return -1;

            if (item2 == null)
                return 1;

            var key1 = this.KeySelector.Invoke(item1);
            var key2 = this.KeySelector.Invoke(item2);
            if (key1 == null && key2 == null)
                return 0;

            if (key1 == null)
                return -1;

            if (key2 == null)
                return 1;

            return key1.CompareTo(key2);
        }
    }

    public static class OrderExtensions {
        public static IQueryable<TModel> Order<TModel, TKey, TProp>(this IQueryable<TModel> query,
            Func<TModel, TProp> keySelector,
            IComparer<TProp> comparer,
            string direction)
            where TModel : class, IMonModel<TKey> {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (keySelector == null)
                return query.OrderByDescending(x => x.Id);

            if (string.IsNullOrEmpty(direction)
                || direction.Equals("desc", StringComparison.CurrentCultureIgnoreCase)) {
                return query.OrderByDescending(keySelector, comparer).AsQueryable();
            } else {
                return query.OrderBy(keySelector, comparer).AsQueryable();
            }
        }

        public static IQueryable<TModel> Order<TModel, TProp>(this IQueryable<TModel> query,
            Func<TModel, TProp> keySelector,
            IComparer<TProp> comparer,
            string direction)
            where TModel : class, IMonModel<string> {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            return query.Order<TModel, string, TProp>(keySelector, comparer, direction);
        }

        public static IQueryable<TModel> Order<TModel, TKey, TProp>(this IQueryable<TModel> query,
            Func<TModel, TProp> keySelector,
            string direction)
            where TModel : class, IMonModel<TKey> {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (keySelector == null)
                return query.OrderByDescending(x => x.Id);

            if (string.IsNullOrEmpty(direction)
                || direction.Equals("desc", StringComparison.CurrentCultureIgnoreCase)) {
                return query.OrderByDescending(keySelector).AsQueryable();
            } else {
                return query.OrderBy(keySelector).AsQueryable();
            }
        }

        public static IQueryable<TModel> Order<TModel, TProp>(this IQueryable<TModel> query,
            Func<TModel, TProp> keySelector,
            string direction)
            where TModel : class, IMonModel<string> {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            return query.Order<TModel, string, TProp>(keySelector, direction);
        }

        public static IQueryable<TModel> Order<TModel, TKey, TProp>(this IQueryable<TModel> query,
            string by,
            IComparer<TProp> comparer,
            string direction)
            where TModel : class, IMonModel<TKey> {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (string.IsNullOrEmpty(by))
                return query.OrderByDescending(x => x.Id);

            return query.Order<TModel, TKey, TProp>((model) => {
                var property = typeof(TModel).GetProperty(by);
                if (property == null)
                    throw new InvalidOperationException($"Cannot get property by name \"{by}\"");

                return (TProp)property.GetValue(model);
            }, comparer, direction);
        }

        public static IQueryable<TModel> Order<TModel, TProp>(this IQueryable<TModel> query,
            string by,
            IComparer<TProp> comparer,
            string direction)
            where TModel : class, IMonModel<string> {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            return query.Order<TModel, string, TProp>(by, comparer, direction);
        }

        public static IQueryable<TModel> Order<TModel, TKey, TProp>(this IQueryable<TModel> query,
            string by,
            string direction)
            where TModel : class, IMonModel<TKey> {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (string.IsNullOrEmpty(by))
                return query.OrderByDescending(x => x.Id);

            return query.Order<TModel, TKey, TProp>((model) => {
                var property = typeof(TModel).GetProperty(by);
                if (property == null)
                    throw new InvalidOperationException($"Cannot get property by name \"{by}\"");

                return (TProp)property.GetValue(model);
            }, direction);
        }

        public static IQueryable<TModel> Order<TModel, TProp>(this IQueryable<TModel> query,
            string by,
            string direction)
            where TModel : class, IMonModel<string> {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            return query.Order<TModel, string, TProp>(by, direction);
        }

        public static IQueryable<TModel> Order<TModel>(this IQueryable<TModel> query,
            string by,
            string direction)
           where TModel : class, IMonModel<string> {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            return query.Order<TModel, object>(by, direction);
        }

        public static IQueryable<TModel1> Order<TModel1, TKey1, TModel2, TKey2, TProperty2>(this IQueryable<TModel1> query,
            Func<TModel1, IMonModelRef<TModel2, TKey2>> selector1,
            Func<TModel2, TProperty2> selector2,
            IMonDatabase db,
            string direction)
            where TModel1 : class, IMonModel<TKey1>
            where TModel2 : class, IMonModel<TKey2>
            where TProperty2 : class, IComparable {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var comparer = new RefComparer<TModel2, TKey2, TProperty2>().SetDB(db).SetKeySelector(selector2);
            return query.Order<TModel1, TKey1, IMonModelRef<TModel2, TKey2>>(selector1, comparer, direction);
        }

        public static IQueryable<TModel1> Order<TModel1, TModel2, TProperty2>(this IQueryable<TModel1> query,
            Func<TModel1, IMonModelRef<TModel2, string>> selector1,
            Func<TModel2, TProperty2> selector2,
            IMonDatabase db,
            string direction)
            where TModel1 : class, IMonModel<string>
            where TModel2 : class, IMonModel<string>
            where TProperty2 : class, IComparable {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            return query.Order<TModel1, string, TModel2, string, TProperty2>(selector1, selector2, db, direction);
        }

        public static IQueryable<TModel1> Order<TModel1, TKey1, TModel2, TKey2, TProperty2>(this IQueryable<TModel1> query,
            Func<TModel1, IMonNodeModelRef<TModel2, TKey2>> selector1,
            Func<TModel2, TProperty2> selector2,
            IMonDatabase db,
            string direction)
            where TModel1 : class, IMonModel<TKey1>
            where TModel2 : class, IMonNodeModel<TModel2, TKey2>
            where TProperty2 : class, IComparable {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var comparer = new NodeRefComparer<TModel2, TKey2, TProperty2>().SetDB(db).SetKeySelector(selector2);
            return query.Order<TModel1, TKey1, IMonNodeModelRef<TModel2, TKey2>>(selector1, comparer, direction);
        }

        public static IQueryable<TModel1> Order<TModel1, TModel2, TProperty2>(this IQueryable<TModel1> query,
            Func<TModel1, IMonNodeModelRef<TModel2, string>> selector1,
            Func<TModel2, TProperty2> selector2,
            IMonDatabase db,
            string direction)
            where TModel1 : class, IMonModel<string>
            where TModel2 : class, IMonNodeModel<TModel2, string>
            where TProperty2 : class, IComparable {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            return query.Order<TModel1, string, TModel2, string, TProperty2>(selector1, selector2, db, direction);
        }
    }
}
