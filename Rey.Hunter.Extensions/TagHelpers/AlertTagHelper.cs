using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Rey.Hunter.TagHelpers {
    [HtmlTargetElement("alert")]
    public class AlertTagHelper : TagHelper {
        public string Type { get; set; } = "danger";
        public string Message { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output) {
            if (string.IsNullOrEmpty(this.Message))
                return;

            var content = new HtmlContentBuilder();
            content.AppendHtmlLine($"<div class=\"alert alert-{this.Type}\">");
            content.AppendHtmlLine("    <button type=\"button\" class=\"close\" data-dismiss=\"alert\">");
            content.AppendHtmlLine("        <span aria-hidden=\"true\">&times;</span>");
            content.AppendHtmlLine("        <span class=\"sr-only\">Close</span>");
            content.AppendHtmlLine("    </button>");
            content.AppendHtmlLine(this.Message);
            content.AppendHtmlLine("</div>");

            output.Content.SetHtmlContent(content);
        }
    }
}
