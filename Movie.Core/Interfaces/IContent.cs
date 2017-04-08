using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Core.Interfaces
{
    public interface IContent
    {
        
        string KeyQuery { get; set; }
        string ID { get; set; }
        string Title { get; set; }
        string Description { get; set; }
        string Director { get; set; }
        string Actor { get; set; }
        string ImageURI { get; set; }
        string TrailerURI { get; set; }
        DateTime ReleaseDate { get; set; }
        string LogoName { get; set; }
        double Rating { get; set; }
        string SourceType { get; set; }
    }
}
