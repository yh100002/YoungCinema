using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Core.Interfaces
{
    public interface ISearchSource
    {
        string ApiKey { get; set; }
        string Filter { get; set; }
        string URL { get; set; }
        IEnumerable<IContent> Search();
    }
}
