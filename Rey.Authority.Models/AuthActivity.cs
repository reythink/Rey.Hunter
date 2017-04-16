using Rey.Authority.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Authority.Models {
    public class AuthActivity : IAuthActivity {
        public string Name { get; set; }
        public string DisplayName { get; set; }

        public AuthActivity(string name, string displayName) {
            this.Name = name;
            this.DisplayName = displayName;
        }

        public AuthActivity(string name)
            : this(name, null) {
        }

        public AuthActivity()
            : this(null, null) {
        }

        public static QueryAuthActivity Query { get; } = new QueryAuthActivity();
        public static DetailAuthActivity Detail { get; } = new DetailAuthActivity();
        public static CreateAuthActivity Create { get; } = new CreateAuthActivity();
        public static UpdateAuthActivity Update { get; } = new UpdateAuthActivity();
        public static DeleteAuthActivity Delete { get; } = new DeleteAuthActivity();
        public static ImportAuthActivity Import { get; } = new ImportAuthActivity();
        public static ExportAuthActivity Export { get; } = new ExportAuthActivity();
        public static DownloadAuthActivity Download { get; } = new DownloadAuthActivity();

        public static UseAuthActivity Use { get; } = new UseAuthActivity();

        public static IEnumerable<IAuthActivity> DataActivities {
            get {
                return new List<IAuthActivity> { Query, Detail, Create, Update, Delete, Import, Export, Download, Use };
            }
        }

        public static IEnumerable<IAuthActivity> FunctionActivities {
            get {
                return new List<IAuthActivity> { Use };
            }
        }

        public static IEnumerable<IAuthActivity> Activities {
            get {
                var list = new List<IAuthActivity>();
                list.AddRange(DataActivities);
                list.AddRange(FunctionActivities);
                return list;
            }
        }
    }

    #region 数据权限

    public abstract class DataAuthActivity : AuthActivity {
        public DataAuthActivity(string name, string displayName)
            : base(name, displayName) {
        }

        public DataAuthActivity(string name)
            : base(name) {
        }
    }

    public class QueryAuthActivity : DataAuthActivity {
        public QueryAuthActivity()
            : base("Query", "查询") {
        }
    }

    public class DetailAuthActivity : DataAuthActivity {
        public DetailAuthActivity()
            : base("Detail", "查看") {
        }
    }

    public class CreateAuthActivity : DataAuthActivity {
        public CreateAuthActivity()
            : base("Create", "新建") {
        }
    }

    public class UpdateAuthActivity : DataAuthActivity {
        public UpdateAuthActivity()
            : base("Update", "更新") {
        }
    }

    public class DeleteAuthActivity : DataAuthActivity {
        public DeleteAuthActivity()
            : base("Delete", "删除") {
        }
    }

    public class ImportAuthActivity : DataAuthActivity {
        public ImportAuthActivity()
            : base("Import", "导入") {
        }
    }

    public class ExportAuthActivity : AuthActivity {
        public ExportAuthActivity()
            : base("Export", "导出") {
        }
    }

    public class DownloadAuthActivity : AuthActivity {
        public DownloadAuthActivity()
            : base("Download", "下载") {
        }
    }

    #endregion 数据权限

    #region 功能性权限

    public abstract class FunctionAuthActivity : AuthActivity {
        public FunctionAuthActivity(string name, string displayName)
            : base(name, displayName) {
        }

        public FunctionAuthActivity(string name)
            : base(name) {
        }
    }

    public class UseAuthActivity : FunctionAuthActivity {
        public UseAuthActivity()
            : base("Use", "使用") {
        }
    }

    #endregion 功能性权限
}
