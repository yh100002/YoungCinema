using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Core.Interfaces
{
    public interface IMoviesRepository : IRepository
    {
        IEnumerable<IContent> SearchMovie(string filter);
        IEnumerable<IContent> SearchMovie(string data_source, string filter, string actor, int from, int to);
        IEnumerable<IContent> GetAllFromCache();
    }
}
