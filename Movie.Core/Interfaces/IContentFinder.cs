﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Core.Interfaces
{
    public interface IContentFinder
    {
        IEnumerable<IContent> Search(ISearchSource online);
    }
}
