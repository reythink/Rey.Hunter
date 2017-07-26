using System;
using System.Collections.Generic;
using System.Text;

namespace Rey.Hunter.Models2.Basic {
    public class Industry : AccountModel {
        public string Name { get; set; }
        public List<ModelRef<Industry>> Children { get; set; } = new List<ModelRef<Industry>>();
    }
}
