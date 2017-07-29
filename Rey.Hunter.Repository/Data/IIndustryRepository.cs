﻿using Rey.Hunter.Models2.Data;
using System.Collections.Generic;

namespace Rey.Hunter.Repository.Data {
    public interface IIndustryRepository : IAccountModelRepository<Industry> {
        IEnumerable<Industry> FindOneWithChildren(string id);
    }
}
