﻿using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Rey.Hunter.Models.Basic;
using System.Collections.Generic;

namespace Rey.Hunter.Api.Basic {
    [Route("/Api/[controller]/")]
    public class LocationController : ReyTreeController<LocationNode, string> {
        protected override LocationNode CreateRootNode() {
            return new LocationNode() { Name = "root", Account = this.CurrentAccount() };
        }

        protected override void DeleteRootNode() {
            this.Collection.DeleteOne(x => x.Account.Id == this.CurrentAccount().Id);
        }

        protected override LocationNode FindRootNode() {
            return this.Collection.FindOne(x => x.Account.Id == this.CurrentAccount().Id);
        }

        protected override List<LocationNode> GetChildNodes(LocationNode node) {
            return node?.Children;
        }

        protected override LocationNode GetUpdateNode(LocationNode oldNode, LocationNode newNode) {
            oldNode.Name = newNode.Name;
            return oldNode;
        }

        protected override void ReplaceRootNode(LocationNode root) {
            this.Collection.UpdateOne(x => x.Account.Id == this.CurrentAccount().Id, builder =>
            builder.Set(x => x.Name, root.Name).Set(x => x.Children, root.Children));
        }
    }
}
