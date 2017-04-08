using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Infrastructure.Configuration
{
    public class MovieElementRetrieverSection : ConfigurationSection
    {
        [ConfigurationProperty("Movies", IsDefaultCollection = true)]
        public MovieElementCollection Movies
        {
            get { return (MovieElementCollection)this["Movies"]; }
            set { this["Movies"] = value; }
        }
    }
}
