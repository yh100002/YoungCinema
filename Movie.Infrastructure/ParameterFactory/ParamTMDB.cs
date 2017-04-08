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
    public class ParamTMDB : Param
    {
        public override IEnumerable<IContent> Search()
        {
            ISearchSource TMDB = new Tmdb() { Filter = this.Filter, Actor = this.Actor, From = this.From, To = this.To };
            Cache cache = new Cache() { Filter = this.Filter, Actor = this.Actor, From = this.From, To = this.To ,Searcher = DataSourceType.TMDB};

            IContentFinder finder = new ContentFinder();

            var cachefound = finder.Search(cache);

            if (cachefound?.Count() > 0)
            {
                return cachefound;
            }
            else
            {
                var tmdbfound = finder.Search(TMDB);
                cache.Set(tmdbfound);
                return tmdbfound;
            }           
        }
    }
}
