using System;

namespace Rey.Hunter.Models2 {
    public interface IModel {
        string Id { get; }
        DateTime? CreateAt { get; }
        DateTime? ModifyAt { get; }

        void UpdateModifyAt();
    }
}

