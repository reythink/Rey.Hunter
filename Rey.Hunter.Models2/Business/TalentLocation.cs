using Rey.Hunter.Models2.Data;
using System.Collections.Generic;

namespace Rey.Hunter.Models2.Business {
    public class TalentLocation {
        public string Current { get; set; }
        public List<LocationRef> Mobility { get; set; } = new List<LocationRef>();
    }
}
