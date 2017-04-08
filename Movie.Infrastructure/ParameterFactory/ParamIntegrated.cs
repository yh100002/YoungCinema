using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Movie.Core.Interfaces;
using Movie.Infrastructure.Handlers;

namespace Movie.Infrastructure.ParameterFactory
{
    public class ParamIntegrated : Param
    {
        public override IEnumerable<IContent> Search()
        {
            ISearchSource TMDB = new Tmdb() { Filter = this.Filter, Actor = this.Actor, From = this.From, To = this.To };
            ISearchSource youtube = new Youtube() { Filter = this.Filter };
            Cache cache = new Cache() { Filter = this.Filter, Actor = this.Actor, From = this.From, To = this.To };

            IContentFinder finder = new ContentFinder();

            var cachefound = finder.Search(cache);

            if (cachefound?.Count() > 0)
            {
                return cachefound;
            }
            else
            {
                var tmdbfound = finder.Search(TMDB);
                var youtubefound = finder.Search(youtube);

                cache.Set(tmdbfound);
                cache.Set(youtubefound);

                var merged = tmdbfound.Concat(youtubefound);

                return merged;
            }
        }
    }
}
