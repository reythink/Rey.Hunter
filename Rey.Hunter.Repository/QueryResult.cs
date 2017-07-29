namespace Rey.Hunter.Repository {
    public class QueryResult {
        public long Count { get; }

        public QueryResult(long count) {
            this.Count = count;
        }

        public override string ToString() {
            return $"[count: {this.Count}]";
        }
    }
}
