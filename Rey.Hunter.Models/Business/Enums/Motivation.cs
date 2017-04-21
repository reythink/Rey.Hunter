using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Hunter.Models.Business {
    public enum Motivation {
        WillingToApply = 1,
        NotConsiderWrongTiming = 2,
        NotConsider = 3,
        NotConsiderInconvenientLocation = 4,
        NotConsiderJobScopeNotAtractive = 5,
        NotConsiderCompanyNotAtractive = 6,
        NotConsiderIndustryNotAtractive = 7,
        NotConsiderBrandNotAtractive = 8,
        NotConsiderChannelNotAtractive = 9,
        NotConsiderDisapprovedLMLeadership = 10,
        NotConsiderDisapprovedCulture = 11,
        NotConsiderCompensationNotAtractive = 12,
        NotConsiderEnglishNotGood = 13,
        NotConsiderPositionIsTooHighToApproch = 14,
        NotConsiderPositionIsTooLowToApproch = 15,
        NotConsiderOppositionFromFamilyOrFriends = 16,
        NotConsiderCompetitor = 17,
        NotConsiderOthers = 9999
    }
}
