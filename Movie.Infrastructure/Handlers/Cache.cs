using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ServiceStack;
using ServiceStack.Text;
using ServiceStack.Redis;
using ServiceStack.DataAnnotations;

using Movie.Core;
using Movie.Core.Enums;
using Movie.Core.Interfaces;
using Movie.Infrastructure.Configuration;
using Movie.Infrastructure.Handlers;

namespace Movie.Infrastructure.Handlers
{
    public class Cache : ISearchSource, IDisposable
    {
        RedisManagerPool redisManager;

        public Cache()
        {
            ApiKey = Loader.GetKey(DataSourceType.CACHE);
            URL = Loader.GetTrailerURL(DataSourceType.CACHE);
            redisManager = new RedisManagerPool(Loader.GetServerAddress(DataSourceType.CACHE));
        }

        public string ApiKey { get; set; }
        public string Filter { get; set; }
        public string URL { get; set; }
        public string Actor { get; set; } = "";
        public int From { get; set; } = 0;
        public int To { get; set; } = 0;
        public DataSourceType Searcher { get; set; } = DataSourceType.NONE;

        public IEnumerable<IContent> Search()
        {
            try
            {
                if (string.IsNullOrEmpty(this.Filter)) return GetAll();

                var redis = redisManager.GetClient();
                var redisMovie = redis.As<IContent>();

                var keys = redisMovie.GetAllKeys();
                IEnumerable<IContent> cachefound1 = redisMovie.GetValues(keys);
                if (Searcher != DataSourceType.NONE)
                {
                    cachefound1 = cachefound1.Where(x => x.SourceType.ToLower().Contains(Searcher.Description().ToLower()));
                }

                var cachefound2 = cachefound1.Where(x =>
                                            x.Title.ToLower().Contains(Filter.ToLower())
                                         || x.Description.ToLower().Contains(Filter.ToLower())
                                         || x.KeyQuery.ToLower().Contains(Filter.ToLower()));

                var cachefound3 = cachefound2.Where(x => x.Actor.ToLower().Contains(this.Actor.ToLower()));

                if (From <= 0 && To <= 0) return cachefound3;

                var cachefound4 = cachefound3.Where(x => (x.ReleaseDate.Year >= this.From && x.ReleaseDate.Year <= this.To));

                return cachefound4;
            }
            catch
            {
                return null; 
            }            
        }

        public void Set(IEnumerable<IContent> set)
        {
            try
            {
                var redis = redisManager.GetClient();
                var redisMovie = redis.As<IContent>();
                set?.Each(x => redisMovie.SetValue(x.ID, x));
                return;
            }
            catch
            {

            }            
        }

        public IEnumerable<IContent> GetAll()
        {
            try
            {
                IContentFinder finder = new ContentFinder();

                var redis = redisManager.GetClient();
                var redisMovie = redis.As<IContent>();

                var keys = redisMovie.GetAllKeys();
                var cachefound = redisMovie.GetValues(keys);
                return cachefound;
            }
            catch
            {
                return null;
            }           
        }

        public void Clear()
        {
            try
            {
                var redis = redisManager.GetClient();
                redis.FlushAll();
            }
            catch
            {

            }
            
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    redisManager.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
