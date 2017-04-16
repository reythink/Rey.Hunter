using Rey.Mon.Attributes;
using Rey.Mon.Models;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Rey.Hunter.Models.Web.Basic;
using System;
using System.Collections.Generic;

namespace Rey.Hunter.Models.Web.Business {
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
}
