using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Movie.Core.Interfaces;

namespace Movie.Infrastructure.ParameterFactory
{
    public abstract class Param
    {
        public string Filter { get; set; }
        public string Actor { get; set; }
        public int  From { get; set; }
        public int To { get; set; }

        public abstract IEnumerable<IContent> Search();
        public virtual void Clear() { }
    }
}
