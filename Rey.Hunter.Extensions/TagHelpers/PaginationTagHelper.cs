using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace Rey.Hunter.TagHelpers {
    public class PaginationData {
        public int Total { get; set; }
        public int Index { get; set; }
        public int Size { get; set; }
        public int PrePages { get; set; }
        public int PostPages { get; set; }

        public PaginationData(int total, int index, int size = 10, int prePages = 4, int postPages = 4) {
            this.Total = total;
            this.Index = index;
            this.Size = size;
            this.PrePages = prePages;
            this.PostPages = postPages;
        }
    }

    public enum PaginationSize {
        Normal,
        Small,
        Large
    }

    public class PaginationText {
        public string First { get; set; } = "First";
        public string Last { get; set; } = "Last";
        public string Previous { get; set; } = "Previous";
        public string Next { get; set; } = "Next";
    }

    [HtmlTargetElement("pagination")]
    public class PaginationTagHelper : TagHelper {
        private IHttpContextAccessor HttpContext { get; }

        public PaginationData Data { get; set; }
        public PaginationSize Size { get; set; }
        public PaginationText Text { get; set; } = new PaginationText();

        private int Pages {
            get { return (int)Math.Ceiling((double)this.Data.Total / (double)this.Data.Size); }
        }

        private string SizeClass {
            get {
                return this.Size == PaginationSize.Normal ? "" : this.Size == PaginationSize.Small ? "pagination-sm" : this.Size == PaginationSize.Large ? "pagination-lg" : "";
            }
        }

        public PaginationTagHelper(IHttpContextAccessor httpContext) {
            this.HttpContext = httpContext;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output) {
            output.TagName = "nav";
            output.Attributes.Add("class", "clearfix");
            output.Content.SetHtmlContent(GenerateHtml());
        }

        private string GenerateUrl(int index) {
            var request = this.HttpContext.HttpContext.Request;
            var query = new QueryString();
            if (!request.Query.ContainsKey("page")) {
                query = request.QueryString.Add("page", index.ToString());
            } else {
                foreach (var key in request.Query.Keys) {
                    if (key.Equals("page", StringComparison.CurrentCultureIgnoreCase)) {
                        query = query.Add("page", index.ToString());
                    } else {
                        query = query.Add(key, request.Query[key]);
                    }
                }
            }
            return $"{request.Path}{query}";
        }

        private IHtmlContent GenerateHtml() {
            var builder = new HtmlContentBuilder();

            builder.AppendHtmlLine($"<ul class=\"pagination {this.SizeClass}\">");

            AppendFirstPage(builder, this.Data);
            AppendPreviousPage(builder, this.Data);
            AppendPage(builder, this.Data);
            AppendNextPage(builder, this.Data);
            AppendLastPage(builder, this.Data);

            builder.AppendHtmlLine("</ul>");

            return builder;
        }

        private void AppendFirstPage(IHtmlContentBuilder builder, PaginationData data) {
            if (data.Index <= 1) {
                builder.AppendHtmlLine($"<li class=\"disabled\"><a href=\"javascript:void(0);\">{this.Text.First}</a></li>");
            } else {
                builder.AppendHtmlLine($"<li><a href=\"{GenerateUrl(1)}\">{this.Text.First}</a></li>");
            }
        }

        private void AppendPreviousPage(IHtmlContentBuilder builder, PaginationData data) {
            if (data.Index <= 1) {
                builder.AppendHtmlLine($"<li class=\"disabled\"><a href=\"javascript:void(0);\">{this.Text.Previous}</a></li>");
            } else {
                builder.AppendHtmlLine($"<li><a href=\"{GenerateUrl(data.Index - 1)}\">{this.Text.Previous}</a></li>");
            }
        }

        private void AppendNextPage(IHtmlContentBuilder builder, PaginationData data) {
            if (data.Index >= this.Pages) {
                builder.AppendHtmlLine($"<li class=\"disabled\"><a href=\"javascript:void(0);\">{this.Text.Next}</a></li>");
            } else {
                builder.AppendHtmlLine($"<li><a href=\"{GenerateUrl(data.Index + 1)}\">{this.Text.Next}</a></li>");
            }
        }

        private void AppendLastPage(IHtmlContentBuilder builder, PaginationData data) {
            if (data.Index >= this.Pages) {
                builder.AppendHtmlLine($"<li class=\"disabled\"><a href=\"javascript:void(0);\">{this.Text.Last}</a></li>");
            } else {
                builder.AppendHtmlLine($"<li><a href=\"{GenerateUrl(this.Pages)}\">{this.Text.Last}</a></li>");
            }
        }

        private void AppendPage(IHtmlContentBuilder builder, PaginationData data) {
            if (data.Index > 1 + data.PrePages) {
                builder.AppendHtmlLine("<li class=\"disabled\"><a href=\"javascript:void(0);\">...</a></li>");
            }

            for (var i = Math.Max(1, data.Index - data.PrePages); i < data.Index; ++i) {
                builder.AppendHtmlLine($"<li><a href=\"{GenerateUrl(i)}\">{i}</a></li>");
            }

            builder.AppendHtmlLine($"<li class=\"active\"><a>{data.Index}</a></li>");

            for (var i = data.Index + 1; i <= Math.Min(this.Pages, data.Index + data.PostPages); ++i) {
                builder.AppendHtmlLine($"<li><a href=\"{GenerateUrl(i)}\">{i}</a></li>");
            }

            if (data.Index < this.Pages - data.PostPages) {
                builder.AppendHtmlLine("<li class=\"disabled\"><a href=\"javascript:void(0);\">...</a></li>");
            }
        }
    }
}
