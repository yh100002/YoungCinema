using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Infrastructure.Configuration
{
    [ConfigurationCollection(typeof(MovieElement))]
    public class MovieElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new MovieElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MovieElement)element).Name;
        }
    }
}
