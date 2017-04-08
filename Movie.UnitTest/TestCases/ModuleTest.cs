using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Linq;

using Movie.Core;
using Movie.Core.Enums;
using Movie.Core.Interfaces;
using Movie.Infrastructure;
using Movie.Infrastructure.Configuration;


namespace Movie.UnitTest
{
    [TestClass]
    public class ModuleTest
    {     

        [TestMethod]
        public void ConfigurationTest()
        {
            //Arrange                    
            MovieElementRetrieverSection config = (MovieElementRetrieverSection)ConfigurationManager.GetSection("MovieElementRetrieverSection");
            var TMDB =  (from MovieElement s in config.Movies
                        where s.Name == DataSourceType.TMDB.Description()
                        select s).FirstOrDefault();
            var YOUTUBE = (from MovieElement s in config.Movies
                          where s.Name == DataSourceType.YOUTUBE.Description()
                           select s).FirstOrDefault();
            //Assert
            Assert.AreEqual(TMDB.Name, DataSourceType.TMDB.Description());
            Assert.AreEqual(YOUTUBE.Name, DataSourceType.YOUTUBE.Description());
        }

        [TestMethod]
        public void YoutubeSearchTest()
        {
            MoviesRepository repo = new MoviesRepository();
            repo.ClearCache();
            var found = repo.SearchMovie(DataSourceType.YOUTUBE.Description(),"terminator 2","",0,0);

            Assert.IsTrue(found.Count() > 0);

        }

        [TestMethod]
        public void TMDBSemanticOnlySearchTest()
        {
            MoviesRepository repo = new MoviesRepository();
            repo.ClearCache();
            var found = repo.SearchMovie(DataSourceType.TMDB.Description(), "terminator 2", "", 0, 0);

            Assert.IsTrue(found.Count() > 0);
        }

        [TestMethod]
        public void TMDBSemanticActorYearSearchTest()
        {
            MoviesRepository repo = new MoviesRepository();
            repo.ClearCache();
            var found = repo.SearchMovie(DataSourceType.TMDB.Description(), "terminator 2", "arnold", 1900, 2000);
            Assert.IsTrue(found.Count() > 0);
        }

        [TestMethod]
        public void CacheSemanticActorYearSearchTest()
        {
            MoviesRepository repo = new MoviesRepository();
            repo.ClearCache();

            var found1 = repo.SearchMovie(DataSourceType.TMDB.Description(), "terminator 2", "arnold", 1900, 2000);

            var found2 = repo.SearchMovie(DataSourceType.CACHE.Description(), "terminator 2", "arnold", 1900, 2000);
            Assert.IsTrue(found1.Count() == found2.Count());

            repo.ClearCache();
        }

        [TestMethod]
        public void CacheClearTest()
        {
            MoviesRepository repo = new MoviesRepository();
            repo.ClearCache();

            var found1 = repo.SearchMovie(DataSourceType.YOUTUBE.Description(), "terminator 2", "", 0, 0);

            var found2 = repo.SearchMovie(DataSourceType.CACHE.Description(), "terminator 2", "", 0, 0);
            Assert.IsTrue((found1.Count() > 0) && (found2.Count() > 0));

            repo.ClearCache();
            found2 = repo.SearchMovie(DataSourceType.CACHE.Description(), "terminater", "", 0, 0);
            Assert.IsTrue(found2.Count() == 0);
        }

    }
}
