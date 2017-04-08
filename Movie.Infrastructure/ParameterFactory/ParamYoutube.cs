using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Movie.Core.Enums;
using Movie.Core.Interfaces;
using Movie.Infrastructure.Handlers;

namespace Movie.Infrastructure.ParameterFactory
{
    public class ParamYoutube : Param
    {
        public override IEnumerable<IContent> Search()
        {
            ISearchSource youtube = new Youtube() { Filter = this.Filter };
            Cache cache = new Cache() { Filter = this.Filter,Searcher = DataSourceType.YOUTUBE};

            IContentFinder finder = new ContentFinder();

            var cachefound = finder.Search(cache);
            
            if (cachefound?.Count() > 0)
            {
                return cachefound;
            }
            else
            {
                var youtubefound = finder.Search(youtube);
                cache.Set(youtubefound);
                return youtubefound;
            }            
        }
    }
}
