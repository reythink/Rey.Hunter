using Rey.Mon.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rey.Mon {
    public class MonNodeRepository<TModel, TKey> : MonRepository<TModel, TKey>, IMonNodeRepository<TModel, TKey>
        where TModel : class, IMonNodeModel<TModel, TKey> {
        public MonNodeRepository(IMonDatabase database, IMongoCollection<TModel> mongoCollection)
            : base(database, mongoCollection) {
        }

        public void InsertNode(TKey parentId, int index, TModel node, int generations = 10) {
            var root = this.FindRootNode(parentId, generations);
            if (root == null)
                return;

            var parent = this.FindNode(root, parentId);
            if (parent == null)
                throw new InvalidOperationException("Cannot find parent!");

            parent.Children.Insert(index, node);
            this.ReplaceOne(x => x.Id.Equals(root.Id), root);
        }

        public void PrependNode(TKey parentId, TModel node, int generations) {
            this.InsertNode(parentId, 0, node, generations);
        }

        public void AppendNode(TKey parentId, TModel node, int generations) {
            var root = this.FindRootNode(parentId, generations);
            if (root == null)
                return;

            var parent = this.FindNode(root, parentId);
            if (parent == null)
                throw new InvalidOperationException("Cannot find parent!");

            parent.Children.Add(node);
            this.ReplaceOne(x => x.Id.Equals(root.Id), root);
        }

        public void ReplaceNode(TKey id, TModel node, int generations) {
            var root = this.FindRootNode(id, generations);
            if (root == null)
                return;

            if (root.Id.Equals(id)) {
                this.ReplaceOne(x => x.Id.Equals(id), node);
                return;
            }

            var parent = this.FindParentNode(root, id);
            if (parent == null)
                throw new InvalidOperationException("Cannot find parent!");

            var index = parent.Children.FindIndex(x => x.Id.Equals(id));
            if (index == -1)
                throw new InvalidOperationException("Cannot find index from parent node!");

            parent.Children[index] = node;
            this.ReplaceOne(x => x.Id.Equals(root.Id), root);
        }

        public void DeleteNode(TKey id, int generations) {
            var root = this.FindRootNode(id, generations);
            if (root == null)
                return;

            if (root.Id.Equals(id)) {
                this.DeleteOne(x => x.Id.Equals(id));
                return;
            }

            var parent = this.FindParentNode(root, id);
            if (parent == null)
                throw new InvalidOperationException("Cannot find parent!");

            parent.Children.RemoveAll(x => x.Id.Equals(id));
            this.ReplaceOne(x => x.Id.Equals(root.Id), root);
        }

        public TModel FindRootNode(TKey id, int generations) {
            var filter = GenRootFilter(id, generations);
            return this.MongoCollection.Find(filter).SingleOrDefault();
        }

        public TModel FindNode(TKey id, int generations) {
            var root = this.FindRootNode(id, generations);
            if (root == null)
                return null;

            return this.FindNode(root, id);
        }

        public TModel FindParentNode(TKey id, int generations) {
            var root = this.FindRootNode(id, generations);
            if (root == null)
                return null;

            if (root.Id.Equals(id))
                return null;

            return this.FindParentNode(root, id);
        }

        protected virtual FilterDefinition<TModel> GenRootFilter(TKey id, int generations) {
            var filters = new List<FilterDefinition<TModel>>() { Builders<TModel>.Filter.Eq(x => x.Id, id) };
            var index = 0;
            for (var filter = Builders<TModel>.Filter.ElemMatch(x => x.Children, x => x.Id.Equals(id));
                index < generations;
                filter = Builders<TModel>.Filter.ElemMatch(x => x.Children, filter), ++index) {
                filters.Add(filter);
            }
            return Builders<TModel>.Filter.Or(filters);
        }

        protected virtual void ForEachNode(TModel root, Func<TModel, bool> each) {
            if (each == null)
                throw new ArgumentNullException(nameof(each));

            var queue = new Queue<TModel>();
            queue.Enqueue(root);

            while (queue.Count > 0) {
                var node = queue.Dequeue();
                if (!each(node))
                    break;

                if (node.Children == null)
                    continue;

                foreach (var child in node.Children) {
                    queue.Enqueue(child);
                }
            }
        }

        protected virtual void ForEachNode(TModel root, Action<TModel> each) {
            if (each == null)
                throw new ArgumentNullException(nameof(each));

            this.ForEachNode(root, x => { each(x); return true; });
        }

        protected virtual TModel FindNode(TModel root, Func<TModel, bool> condition) {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));

            TModel found = null;
            this.ForEachNode(root, x => {
                if (condition(x)) {
                    found = x;
                    return false;
                }
                return true;
            });
            return found;
        }

        protected virtual TModel FindNode(TModel root, TKey id) {
            return this.FindNode(root, x => x.Id.Equals(id));
        }

        protected virtual TModel FindParentNode(TModel root, Func<TModel, bool> condition) {
            TModel found = null;
            this.ForEachNode(root, node => {
                if (node.Children == null || node.Children.Count == 0)
                    return true;

                if (node.Children.Any(condition)) {
                    found = node;
                    return false;
                }
                return true;
            });
            return found;
        }

        protected virtual TModel FindParentNode(TModel root, TKey id) {
            return this.FindParentNode(root, x => x.Id.Equals(id));
        }
    }
}