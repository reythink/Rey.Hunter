using System;
using System.Collections.Generic;
using System.Text;

namespace Rey.Hunter.TagHelpers.Pagination {
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
}
