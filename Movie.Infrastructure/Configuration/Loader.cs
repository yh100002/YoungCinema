using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using Movie.Core.Enums;

namespace Movie.Infrastructure.Configuration
{
    public class Loader
    {
        static MovieElementRetrieverSection _config = (MovieElementRetrieverSection)ConfigurationManager.GetSection("MovieElementRetrieverSection");
        public static string GetKey(DataSourceType type)
        {
            var movie = (from MovieElement s in _config.Movies
                        where s.Name == type.Description().ToLower()
                         select s).FirstOrDefault();
            return movie?.Key;
        }

        public static string GetTrailerURL(DataSourceType type)
        {
            var movie = (from MovieElement s in _config.Movies
                         where s.Name == type.Description().ToLower()
                         select s).FirstOrDefault();
            return movie?.Trailerurl;
        }

        public static string GetServerAddress(DataSourceType type)
        {
            var movie = (from MovieElement s in _config.Movies
                         where s.Name == type.Description().ToLower()
                         select s).FirstOrDefault();
            return movie?.Server;
        }
    }
}
