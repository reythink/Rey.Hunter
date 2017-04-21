using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Rey.Hunter.Models.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Hunter.Api.Basic {
    [Route("/Api/[controller]/")]
    public class CategoryController : ReyTreeController<CategoryNode, string> {
        protected override CategoryNode CreateRootNode() {
            return new CategoryNode() { Name = "root", Account = this.CurrentAccount() };
        }

        protected override void DeleteRootNode() {
            this.Collection.DeleteOne(x => x.Account.Id == this.CurrentAccount().Id);
        }

        protected override CategoryNode FindRootNode() {
            return this.Collection.FindOne(x => x.Account.Id == this.CurrentAccount().Id);
        }

        protected override List<CategoryNode> GetChildNodes(CategoryNode node) {
            return node?.Children;
        }

        protected override CategoryNode GetUpdateNode(CategoryNode oldNode, CategoryNode newNode) {
            oldNode.Name = newNode.Name;
            return oldNode;
        }

        protected override void ReplaceRootNode(CategoryNode root) {
            this.Collection.UpdateOne(x => x.Account.Id == this.CurrentAccount().Id, builder => builder
            .Set(x => x.Name, root.Name)
            .Set(x => x.Children, root.Children));
        }
    }
}
