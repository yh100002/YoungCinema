using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Movie.Core.Interfaces;

namespace Movie.Core
{
    public class VideoBase : IContent
    {
        public string KeyQuery { get; set; } = "";
        public string ID { get; set; } = "";
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Director { get; set; } = "";
        public string Actor { get; set; } = "";
        public string ImageURI { get; set; } = "";
        public string TrailerURI { get; set; } = "";
        public DateTime ReleaseDate { get; set; } = DateTime.Now;
        public string LogoName { get; set; } = "youtube_logo";
        public double Rating { get; set; } = 0.0;
        public string SourceType { get; set; } = "";
    }
}
