using MongoDB.Bson.Serialization.Attributes;

namespace Rey.Hunter.Models2.Business {
    public class ProjectQuestion {
        /// <summary>
        /// Reporting Line:
        /// </summary>
        [BsonIgnoreIfNull]
        public string Question1 { get; set; }

        /// <summary>
        /// Line manager’s background and style?
        /// </summary>
        [BsonIgnoreIfNull]
        public string Question2 { get; set; }

        /// <summary>
        /// Subbordidate:
        /// </summary>
        [BsonIgnoreIfNull]
        public string Question3 { get; set; }

        /// <summary>
        /// Package Range & Salary Sturcture:
        /// </summary>
        [BsonIgnoreIfNull]
        public string Question4 { get; set; }

        /// <summary>
        /// Why this opening:
        /// </summary>
        [BsonIgnoreIfNull]
        public string Question5 { get; set; }

        /// <summary>
        /// How many candidates have been interviewed? Why failed? 
        /// </summary>
        [BsonIgnoreIfNull]
        public string Question6 { get; set; }

        /// <summary>
        /// Key responsibilities:
        /// </summary>
        [BsonIgnoreIfNull]
        public string Question7 { get; set; }

        /// <summary>
        /// Requirements / Top-3 skills needed:
        /// </summary>
        [BsonIgnoreIfNull]
        public string Question8 { get; set; }

        /// <summary>
        /// Expectation/Key challenges: 
        /// </summary>
        [BsonIgnoreIfNull]
        public string Question9 { get; set; }

        /// <summary>
        /// Preferred company background: 
        /// </summary>
        [BsonIgnoreIfNull]
        public string Question10 { get; set; }

        /// <summary>
        /// Selling points:
        /// </summary>
        [BsonIgnoreIfNull]
        public string Question11 { get; set; }

        /// <summary>
        /// Interview process:
        /// </summary>
        [BsonIgnoreIfNull]
        public string Question12 { get; set; }
    }
}
