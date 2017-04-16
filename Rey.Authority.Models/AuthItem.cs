using Rey.Authority.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Authority.Models {
    public class AuthItem : IAuthItem {
        public IAuthTarget Target { get; }
        public List<IAuthActivity> Activities { get; } = new List<IAuthActivity>();

        public AuthItem(IAuthTarget target, IEnumerable<IAuthActivity> activities)
            : this(target) {
            this.Activities.AddRange(activities);
        }

        public AuthItem(IAuthTarget target, params IAuthActivity[] activities)
            : this(target) {
            this.Activities.AddRange(activities);
        }

        public AuthItem(IAuthTarget target) {
            this.Target = target;
        }

        public AuthItem() {
        }

        public IAuthTarget GetTarget() {
            return this.Target;
        }

        public IEnumerable<IAuthActivity> GetActivities() {
            return this.Activities;
        }
    }
}
