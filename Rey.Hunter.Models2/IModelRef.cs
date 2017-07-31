using System;

namespace Rey.Hunter.Models2 {
    public interface IModelRef {
        string Id { get; }
        DateTime? UpdateAt { get; }
    }
}
