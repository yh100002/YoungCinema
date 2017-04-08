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
    public class ParamCache : Param
    {
        public override IEnumerable<IContent> Search()
        {            
            Cache cache = new Cache() { Filter = this.Filter, Actor = this.Actor, From = this.From, To = this.To };

            IContentFinder finder = new ContentFinder();

            try
            {
                var cachefound = finder.Search(cache);

                return cachefound;
            }
            catch
            {
                return null;
            }            
        }

        public override void Clear()
        {
            base.Clear();
            Cache cache = new Cache() { Filter = this.Filter, Actor = this.Actor, From = this.From, To = this.To };
            cache.Clear();
        }
    }
}
