using Rey.Mon.Attributes;
using Rey.Mon.Models;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Rey.Hunter.Models.Basic;
using System;
using System.Collections.Generic;

namespace Rey.Hunter.Models.Business {
    [MonCollection("bus.projects")]
    public class Project : AccountModel {
        public string Name { get; set; }

        public int? Headcount { get; set; }

        public MonStringModelRef<Company> Client { get; set; }

        public List<MonStringNodeModelRef<FunctionNode>> Functions { get; set; } = new List<MonStringNodeModelRef<FunctionNode>>();

        public List<MonStringNodeModelRef<LocationNode>> Locations { get; set; } = new List<MonStringNodeModelRef<LocationNode>>();

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? StartDate { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? AssignmentDate { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? OnBoardDate { get; set; }

        public string Notes { get; set; }

        public List<Candidate> Candidates { get; set; } = new List<Candidate>();

        public ProjectJobUnderstanding JobUnderstanding { get; set; }
    }

    public enum CandidateStatus {
        Approching = 1,
        Approched = 2,
        Shortlisted = 3,
        Interviewed = 4,
        Offering = 5,
        OfferAccepted = 6,
        OnBoard = 7,
        Failed = 9999,
    }

    public class InterviewInfo {

    }

    public class Candidate {
        public MonModelRef<Talent, string> Talent { get; set; }

        public CandidateStatus Status { get; set; } = CandidateStatus.Approching;

        public List<InterviewInfo> Interviews { get; set; } = new List<InterviewInfo>();
    }

    public class ProjectJobUnderstanding {
        /// <summary>
        /// Reporting Line:
        /// </summary>
        public string Field1 { get; set; }

        /// <summary>
        /// Line manager’s background and style?
        /// </summary>
        public string Field2 { get; set; }

        /// <summary>
        /// Subbordidate:
        /// </summary>
        public string Field3 { get; set; }

        /// <summary>
        /// Package Range & Salary Sturcture:
        /// </summary>
        public string Field4 { get; set; }

        /// <summary>
        /// Why this opening:
        /// </summary>
        public string Field5 { get; set; }

        /// <summary>
        /// How many candidates have been interviewed? Why failed? 
        /// </summary>
        public string Field6 { get; set; }

        /// <summary>
        /// Key responsibilities:
        /// </summary>
        public string Field7 { get; set; }

        /// <summary>
        /// Requirements / Top-3 skills needed:
        /// </summary>
        public string Field8 { get; set; }

        /// <summary>
        /// Expectation/Key challenges: 
        /// </summary>
        public string Field9 { get; set; }

        /// <summary>
        /// Preferred company background: 
        /// </summary>
        public string Field10 { get; set; }

        /// <summary>
        /// Selling points:
        /// </summary>
        public string Field11 { get; set; }

        /// <summary>
        /// Interview process:
        /// </summary>
        public string Field12 { get; set; }
    }
}
