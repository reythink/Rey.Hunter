using Rey.Mon;
using Rey.Mon.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc {
    public abstract class ReyTreeController<TNode, TKey> : ReyController
        where TNode : class, IMonModel<TKey> {
        protected virtual IMonCollection<TNode> Collection {
            get { return this.GetMonCollection<TNode>(); }
        }

        protected virtual TNode Root {
            get {
                var root = this.FindRootNode();
                if (root == null) {
                    root = this.CreateRootNode();
                    this.Collection.InsertOne(root);
                }
                return root;
            }
        }

        [HttpGet]
        public Task<IActionResult> Get() {
            return JsonInvokeOneAsync(() => {
                return this.Root;
            });
        }

        [HttpGet("{id}")]
        public Task<IActionResult> Get(TKey id) {
            return JsonInvokeOneAsync(() => {
                return this.FindNode(id);
            });
        }

        [HttpGet("{id}/parent")]
        public Task<IActionResult> GetParent(TKey id) {
            return JsonInvokeOneAsync(() => {
                return this.FindParentNode(id);
            });
        }

        [HttpGet("{id}/children")]
        public Task<IActionResult> GetChildren(TKey id) {
            return JsonInvokeManyAsync(() => {
                return this.GetChildNodes(this.FindNode(id));
            });
        }

        [HttpPost]
        public Task<IActionResult> Insert([FromBody]TNode node) {
            return JsonInvokeOneAsync(() => {
                var root = this.Root;
                this.GetChildNodes(root).Add(node);
                this.ReplaceRootNode(root);
                return node;
            });
        }

        [HttpPost("{id}")]
        public Task<IActionResult> Insert(TKey id, [FromBody]TNode node) {
            return JsonInvokeOneAsync(() => {
                var root = this.Root;
                this.GetChildNodes(this.FindNode(root, id)).Add(node);
                this.ReplaceRootNode(root);
                return node;
            });
        }

        [HttpPut]
        public Task<IActionResult> Update([FromBody]TNode node) {
            return JsonInvokeAsync(() => {
                this.ReplaceRootNode(GetUpdateNode(this.Root, node));
            });
        }

        [HttpPut("{id}")]
        public Task<IActionResult> Update(TKey id, [FromBody]TNode node) {
            return JsonInvokeAsync(() => {
                node.Id = id;
                var root = this.Root;
                if (id.Equals(root.Id)) {
                    this.ReplaceRootNode(node);
                    return;
                }

                var parent = this.FindParentNode(root, id);
                if (parent == null)
                    throw new InvalidOperationException("Invalid id!");

                var children = this.GetChildNodes(parent);
                var index = children.FindIndex(x => x.Id.Equals(id));
                var updateNode = GetUpdateNode(children[index], node);
                //children.RemoveAt(index);
                //children.Insert(index, updateNode);
                children[index] = updateNode;
                this.ReplaceRootNode(root);
            });
        }

        [HttpDelete]
        public Task<IActionResult> Delete() {
            return JsonInvokeAsync(() => {
                this.DeleteRootNode();
            });
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> Delete(TKey id) {
            return JsonInvokeAsync(() => {
                var root = this.Root;
                if (id.Equals(root.Id)) {
                    this.DeleteRootNode();
                    return;
                }

                var parent = this.FindParentNode(root, id);
                if (parent == null)
                    throw new InvalidOperationException("Invalid id!");

                this.GetChildNodes(parent).RemoveAll(x => x.Id.Equals(id));
                this.ReplaceRootNode(root);
            });
        }

        protected abstract TNode FindRootNode();

        protected abstract TNode CreateRootNode();

        protected abstract void DeleteRootNode();

        protected abstract void ReplaceRootNode(TNode root);

        protected abstract TNode GetUpdateNode(TNode oldNode, TNode newNode);

        protected abstract List<TNode> GetChildNodes(TNode node);

        protected virtual void ForEachNode(TNode root, Func<TNode, bool> each) {
            if (root == null)
                throw new ArgumentNullException(nameof(root));

            if (each == null)
                throw new ArgumentNullException(nameof(each));

            var queue = new Queue<TNode>();
            queue.Enqueue(root);

            while (queue.Count > 0) {
                var node = queue.Dequeue();
                if (!each(node))
                    break;

                var children = this.GetChildNodes(node);
                if (children == null)
                    continue;

                foreach (var child in children) {
                    queue.Enqueue(child);
                }
            }
        }

        protected virtual void ForEachNode(Func<TNode, bool> each) {
            if (each == null)
                throw new ArgumentNullException(nameof(each));

            this.ForEachNode(this.Root, each);
        }

        protected virtual void ForEachNode(TNode root, Action<TNode> each) {
            if (root == null)
                throw new ArgumentNullException(nameof(root));

            if (each == null)
                throw new ArgumentNullException(nameof(each));

            this.ForEachNode(root, node => {
                each(node);
                return true;
            });
        }

        protected virtual void ForEachNode(Action<TNode> each) {
            if (each == null)
                throw new ArgumentNullException(nameof(each));

            this.ForEachNode(this.Root, each);
        }

        protected virtual TNode FindNode(TNode root, TKey id) {
            if (root == null)
                throw new ArgumentNullException(nameof(root));

            TNode found = null;
            this.ForEachNode(root, node => {
                if (id.Equals(node.Id)) {
                    found = node;
                    return false;
                }
                return true;
            });
            return found;
        }

        protected virtual TNode FindNode(TKey id) {
            return this.FindNode(this.Root, id);
        }

        protected virtual TNode FindParentNode(TNode root, TKey id) {
            if (root == null)
                throw new ArgumentNullException(nameof(root));

            TNode found = null;
            this.ForEachNode(root, node => {
                var children = this.GetChildNodes(node);
                if (children == null)
                    return true;

                if (children.Any(child => id.Equals(child.Id))) {
                    found = node;
                    return false;
                }
                return true;
            });
            return found;
        }

        protected virtual TNode FindParentNode(TKey id) {
            return this.FindParentNode(this.Root, id);
        }
    }
}
