using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Movie.Core.Interfaces;

namespace Movie.Infrastructure
{
    public class ContentFinder : IContentFinder
    {
        public IEnumerable<IContent> Search(ISearchSource online)
        {
            return online.Search();
        }
    }
}
