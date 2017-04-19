using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rey.Hunter.TagHelpers.Hash {
    [HtmlTargetElement("script", Attributes = "hash")]
    public class ScriptHashTagHelper : TagHelper {
        private IHostingEnvironment Hosting { get; }
        public string Src { get; set; }
        public string Format { get; set; }

        public ScriptHashTagHelper(IHostingEnvironment hosting) {
            this.Hosting = hosting;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output) {
            var hash = HashUtility.Hash(this.Hosting.WebRootPath, this.Src);
            var src = this.Src;
            if (!string.IsNullOrEmpty(hash)) {
                if (string.IsNullOrEmpty(this.Format)) {
                    src = string.Format("{0}?hash={1}", this.Src, hash);
                } else {
                    src = this.Format;
                    src = src.Replace("{src}", this.Src);
                    src = src.Replace("{hash}", hash);
                }
            }
            output.Attributes.SetAttribute("src", src);
            output.Attributes.Remove(output.Attributes["hash"]);
        }
    }
}
