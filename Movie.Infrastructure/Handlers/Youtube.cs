using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

using Movie.Core;
using Movie.Core.Interfaces;
using Movie.Core.Enums;
using Movie.Infrastructure.Configuration;

namespace Movie.Infrastructure.Handlers
{
    public class Youtube : ISearchSource
    {
        public Youtube()
        {
            ApiKey = Loader.GetKey(DataSourceType.YOUTUBE);
            URL = Loader.GetTrailerURL(DataSourceType.YOUTUBE);
        }
        public string ApiKey { get; set; }
        public string Filter { get; set; }
        public string URL { get; set; }
                

        public IEnumerable<IContent> Search()
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = this.ApiKey,
                ApplicationName = this.GetType().ToString()
            });

            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = Filter; // "love | couple - woman" ===> detailed semantic search
            searchListRequest.MaxResults = 50;

            var searchListResponse = searchListRequest.Execute();
            List<VideoBase> videos = new List<VideoBase>();
            foreach (var searchResult in searchListResponse.Items)
            {
                if (searchResult.Id.Kind.Contains("youtube#video") == true)
                {
                    var video = new VideoBase
                    {
                        ID = searchResult.Id.VideoId,
                        KeyQuery = Filter,
                        Title = searchResult.Snippet.Title,
                        Description = searchResult.Snippet.Description,
                        TrailerURI = string.Format(URL.Trim() + "{0}", searchResult.Id.VideoId),
                        ImageURI = searchResult.Snippet.Thumbnails.High.Url,
                        ReleaseDate = Convert.ToDateTime(searchResult.Snippet.PublishedAt),
                        Rating = SimulatedRating(),
                        SourceType = DataSourceType.YOUTUBE.Description()
                    };

                    videos.Add(video);
                }
            }

            return videos;
        }

        private readonly Random _random = new Random();
        private double SimulatedRating()
        {           
            double rating = _random.Next(10, 51) * 0.1;
            return rating;
        }

    }
}
