using Rey.Hunter.Models2.Attributes;
using Rey.Hunter.Models2.Data;
using System;
using System.Collections.Generic;

namespace Rey.Hunter.Models2.Business {
    [MongoCollection("busi.project")]
    public class Project : AccountModel {
        public string Name { get; set; }
        public int? Headcount { get; set; }
        public CompanyRef Client { get; set; }

        public UserRef Manager { get; set; }
        public List<UserRef> Consultant { get; set; } = new List<UserRef>();

        public List<FunctionRef> Function { get; set; } = new List<FunctionRef>();
        public List<LocationRef> Location { get; set; } = new List<LocationRef>();

        public DateTime? AssignmentDate { get; set; }
        public DateTime? OfferSignedDate { get; set; }
        public DateTime? OnBoardDate { get; set; }

        public string Notes { get; set; }

        public List<ProjectCandidate> Candidate { get; set; } = new List<ProjectCandidate>();
        public ProjectQuestion Question { get; set; } = new ProjectQuestion();
        public List<Attachment> Attachments { get; set; } = new List<Attachment>();
    }
}
