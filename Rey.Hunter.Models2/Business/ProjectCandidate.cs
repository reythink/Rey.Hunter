using System.Collections.Generic;

namespace Rey.Hunter.Models2.Business {
    public class ProjectCandidate {
        public TalentRef Talent { get; set; }
        public CandidateStatus Status { get; set; } = CandidateStatus.Approching;
        public List<CandidateInterviewItem> Interviews { get; set; } = new List<CandidateInterviewItem>();
    }
}
