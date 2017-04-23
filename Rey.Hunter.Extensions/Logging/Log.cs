using Rey.Hunter.Models;
using Rey.Hunter.Models.Identity;
using Rey.Mon;
using Rey.Mon.Attributes;
using Rey.Mon.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rey.Hunter.ModelLogging {
    [MonCollection("log.logs")]
    public class Log<TModel, TKey> : Model
        where TModel : class, IMonModel<TKey> {
        public MonStringModelRef<User> User { get; set; }
        public LogAction Action { get; set; }
        public MonModelRef<TModel, TKey> Model { get; set; }
    }
}
