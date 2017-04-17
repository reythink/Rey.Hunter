using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Rey.Hunter.Models.Web.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Hunter.Api.Basic {
    [Route("/Api/[controller]/")]
    public class ChannelController : ReyTreeController<ChannelNode, string> {
        protected override ChannelNode CreateRootNode() {
            return new ChannelNode() { Name = "root", Account = this.CurrentAccount() };
        }

        protected override void DeleteRootNode() {
            this.Collection.DeleteOne(x => x.Account.Id == this.CurrentAccount().Id);
        }

        protected override ChannelNode FindRootNode() {
            return this.Collection.FindOne(x => x.Account.Id == this.CurrentAccount().Id);
        }

        protected override List<ChannelNode> GetChildNodes(ChannelNode node) {
            return node?.Children;
        }

        protected override ChannelNode GetUpdateNode(ChannelNode oldNode, ChannelNode newNode) {
            oldNode.Name = newNode.Name;
            return oldNode;
        }

        protected override void ReplaceRootNode(ChannelNode root) {
            this.Collection.UpdateOne(x => x.Account.Id == this.CurrentAccount().Id, builder => builder
            .Set(x => x.Name, root.Name)
            .Set(x => x.Children, root.Children));
        }
    }
}
