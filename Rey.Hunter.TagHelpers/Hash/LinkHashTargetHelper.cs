using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Rey.Hunter.TagHelpers.Hash {
    [HtmlTargetElement("link", Attributes = "hash")]
    public class LinkHashTargetHelper : TagHelper {
        private IHostingEnvironment Hosting { get; }
        public string Href { get; set; }
        public string Format { get; set; }

        public LinkHashTargetHelper(IHostingEnvironment hosting) {
            this.Hosting = hosting;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output) {
            var hash = HashUtility.Hash(this.Hosting.WebRootPath, this.Href);
            var href = this.Href;
            if (!string.IsNullOrEmpty(hash)) {
                if (string.IsNullOrEmpty(this.Format)) {
                    href = string.Format("{0}?hash={1}", this.Href, hash);
                } else {
                    href = this.Format;
                    href = href.Replace("{href}", this.Href);
                    href = href.Replace("{hash}", hash);
                }
            }
            output.Attributes.SetAttribute("href", href);
            output.Attributes.Remove(output.Attributes["hash"]);
        }
    }
}
