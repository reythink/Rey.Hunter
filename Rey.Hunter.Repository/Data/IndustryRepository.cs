using System;
using System.Collections.Generic;
using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Data;
using System.Linq;

namespace Rey.Hunter.Repository.Data {
    public class IndustryRepository : AccountModelRepositoryBase<Industry>, IIndustryRepository {
        public IndustryRepository(IRepositoryManager manager, Account account)
            : base(manager, account) {
        }

        public IEnumerable<Industry> FindOneWithChildren(string id) {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var results = new List<Industry>();
            var root = this.FindOne(id);
            if (root == null)
                return results;

            var stack = new Stack<Industry>();
            stack.Push(root);

            while (stack.Count > 0) {
                var node = stack.Pop();
                results.Add(node);

                if (node.Children != null && node.Children.Count > 0) {
                    (node.Children as IEnumerable<string>).Reverse().ToList().ForEach(x => stack.Push(this.FindOne(x)));
                }
            }

            return results;
        }
    }
}
