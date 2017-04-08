using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using Newtonsoft.Json;
using Movie.Core;
using Movie.Core.Enums;
using Movie.Core.Interfaces;
using Movie.Infrastructure.Configuration;

namespace Movie.Infrastructure.Handlers
{
    public class Tmdb : ISearchSource
    {
        public Tmdb()
        {
            ApiKey = Loader.GetKey(DataSourceType.TMDB);
            URL = Loader.GetTrailerURL(DataSourceType.TMDB);
        }

        public string ApiKey { get; set; }
        public string Filter { get; set; } = "";
        public string URL { get; set; } = "";
        public string Actor { get; set; } = "";
        public int From { get; set; } = 0;
        public int To { get; set; } = 0;
                

        public IEnumerable<IContent> Search()
        {
            TMDbClient client = new TMDbClient(ApiKey);

            try
            {
                FetchConfig(client);                
            }
            catch
            {
                client.GetConfig();
            }
            
            return FetchMovie(client);
        }

        private void FetchConfig(TMDbClient client)
        {
            FileInfo configJson = new FileInfo("config-cache.json");

            if (configJson.Exists && configJson.LastWriteTimeUtc >= DateTime.UtcNow.AddHours(-1))
            {
                string json = File.ReadAllText(configJson.FullName, Encoding.UTF8);
                client.SetConfig(JsonConvert.DeserializeObject<TMDbConfig>(json));
            }
            else
            {
                client.GetConfig();
                string json = JsonConvert.SerializeObject(client.Config);
                File.WriteAllText(configJson.FullName, json, Encoding.UTF8);
            }
        }

        private IEnumerable<MovieBase> FetchMovie(TMDbClient client)
        {
            SearchContainer<SearchMovie> results = client.SearchMovieAsync(query:Filter).Result;
            List<MovieBase> movies = new List<MovieBase>();
            foreach (SearchMovie result in results.Results)
            {
                TMDbLib.Objects.Movies.Movie movie = client.GetMovieAsync(result.Id, MovieMethods.Videos | MovieMethods.Credits).Result;
                Crew crew = movie.Credits.Crew.FirstOrDefault(job => job.Job.ToLower().Contains("director"));
                Cast cast = movie.Credits.Cast.FirstOrDefault(x => x.Name.ToLower().Contains(this.Actor.ToLower())); ;
                string actor = "";
                actor = cast?.Name;
                if (string.IsNullOrEmpty(actor))
                {   
                    if (cast == null) continue;                    
                }

                int year = Convert.ToDateTime(movie.ReleaseDate).Year;

                if (From > 0 && To > 0)
                {
                    if (!(From <= year && To >= year))
                    {
                        continue;
                    }
                }

                var tmdb_movie = new MovieBase()
                {
                    ID = movie.Id.ToString(),
                    Title = movie.Title,
                    KeyQuery = Filter,
                    Description = movie.Overview,
                    ReleaseDate = Convert.ToDateTime(movie.ReleaseDate),
                    ImageURI = client.GetImageUrl("original", movie.PosterPath).ToString(),
                    Rating = movie.VoteAverage / 2.0,
                    Director = crew?.Name,
                    Actor = actor,
                    SourceType = DataSourceType.TMDB.Description()
                };

                try
                {
                    tmdb_movie.TrailerURI = string.Format(URL.Trim() + "{0}", movie.Videos.Results.FirstOrDefault().Key);
                }
                catch
                {
                    tmdb_movie.TrailerURI = "";
                }
                

                movies.Add(tmdb_movie);
            }

            return movies;
        }
    }
}
