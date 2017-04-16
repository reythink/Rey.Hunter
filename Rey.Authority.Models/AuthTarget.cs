using Rey.Authority.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Authority.Models {
    public class AuthTarget : IAuthTarget {
        public string Name { get; set; }
        public string DisplayName { get; set; }

        public AuthTarget(string name, string displayName) {
            this.Name = name;
            this.DisplayName = displayName;
        }

        public AuthTarget(string name)
            : this(name, null) {
        }

        public AuthTarget()
            : this(null, null) {
        }

        public static CompanyTarget Company { get; } = new CompanyTarget();
        public static ClientTarget Client { get; } = new ClientTarget();
        public static CandidateTarget Candidate { get; } = new CandidateTarget();
        public static CandidateGroupTarget CandidateGroup { get; } = new CandidateGroupTarget();
        public static ProjectTarget Project { get; } = new ProjectTarget();

        public static RoleAuthTarget Role { get; } = new RoleAuthTarget();
        public static UserAuthTarget User { get; } = new UserAuthTarget();
    }

    public class CompanyTarget : AuthTarget {
        public CompanyTarget()
            : base("Company", "公司") {
        }
    }

    public class ClientTarget : AuthTarget {
        public ClientTarget()
            : base("Client", "客户") {
        }
    }

    public class CandidateTarget : AuthTarget {
        public CandidateTarget()
            : base("Candidate", "候选人") {
        }
    }

    public class CandidateGroupTarget : AuthTarget {
        public CandidateGroupTarget()
            : base("CandidateGroup", "候选人组") {
        }
    }

    public class ProjectTarget : AuthTarget {
        public ProjectTarget()
            : base("Project", "项目") {
        }
    }

    public class RoleAuthTarget : AuthTarget {
        public RoleAuthTarget()
            : base("Role", "角色") {
        }
    }

    public class UserAuthTarget : AuthTarget {
        public UserAuthTarget()
            : base("User", "用户") {
        }
    }
}
