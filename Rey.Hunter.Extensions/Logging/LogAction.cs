using Rey.Mon;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rey.Hunter.ModelLogging {
    public class LogAction {
        public string Name { get; set; }
        public LogAction(string name) {
            this.Name = name;
        }

        public override string ToString() {
            return this.Name;
        }

        public static LogAction Create { get; } = new LogAction("Create");
        public static LogAction Update { get; } = new LogAction("Update");
        public static LogAction Delete { get; } = new LogAction("Delete");
    }
}
