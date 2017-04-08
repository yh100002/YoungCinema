using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Infrastructure.Configuration
{
    public class MovieElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("trailerurl", IsRequired = true, DefaultValue = "https://www.youtube.com/watch?v=")]
        [RegexStringValidator(@"https?\://\S+")]
        public string Trailerurl
        {
            get { return (string)this["trailerurl"]; }
            set { this["trailerurl"] = value; }
        }

        [ConfigurationProperty("key", IsRequired = true, DefaultValue = "")]
        public string Key
        {
            get { return (string)this["key"]; }
            set { this["key"] = value; }
        }

        [ConfigurationProperty("server")]
        public string Server
        {
            get { return (string)this["server"]; }
            set { this["server"] = value; }
        }
    }
}
