using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Movie.Core.Enums;

namespace Movie.Infrastructure.ParameterFactory
{
    public class ParamCreator
    {       
        public Param Create(DataSourceType sourceType, string filter, string actor, int from, int to)
        {            
            
            if (sourceType == DataSourceType.TMDB)
            {
                ParamTMDB tmdb = new ParamTMDB()
                {
                    Filter = filter,
                    Actor = actor,
                    From = from,
                    To = to
                };
                return tmdb;       
            }
            else if (sourceType == DataSourceType.YOUTUBE)
            {
                ParamYoutube youtube = new ParamYoutube()
                {
                    Filter = filter                  
                };
                return youtube;
            }
            else if (sourceType == DataSourceType.CACHE)
            {
                ParamCache cache = new ParamCache()
                {
                    Filter = filter,
                    Actor = actor,
                    From = from,
                    To = to
                };
                return cache;
            }
            else if (sourceType == DataSourceType.INTEGRATED)
            {
                ParamIntegrated all = new ParamIntegrated()
                {
                    Filter = filter,
                    Actor = actor,
                    From = from,
                    To = to
                };
                return all;
            }
            else
            {
                return null;
            }                      
        }
    }
}
