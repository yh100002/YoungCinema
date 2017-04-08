using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using NLog;

using Movie.Core;
using Movie.Core.Interfaces;
using Movie.Infrastructure;
using Movie.Web.Response;

namespace Movie.Web.Controllers
{
    [HandleError()]
    [RoutePrefix("movies")]
    [Route("{action = index}")]
    public class MoviesController : Controller
    {
        private readonly IContentsRepository _moviesRepository;
        Logger logger = LogManager.GetCurrentClassLogger();

        public MoviesController(IContentsRepository repo)
        {
            _moviesRepository = repo;
        }

        // GET: Movies        
        public ActionResult Index()
        {            
            return View();
        }
                
        public JsonNetResult FromRedisCacheAll()
        {
            JsonNetResult jsonResponse = new JsonNetResult()
            {
                Data = _moviesRepository.GetAllFromCache(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };        

            return jsonResponse;
        }

        [Route("{page:int=0}/{pageSize=3}/{filter?}/{actor?}/{from:int=0}/{to:int=0}")]
        public JsonNetResult Pages(int? page, int? pageSize, string source="", string filter = "", string actor="", int from=0, int to=0)
        {            

            int currentPage = page.Value;
            int currentPageSize = pageSize.Value;
            int totalMovies = 0;
            IEnumerable<IContent> source_movies;
            IEnumerable<IContent> paged_movies;
            string data_source = source.ToLower();            
            
            if (!string.IsNullOrEmpty(filter))
            {
                source_movies = _moviesRepository.SearchMovie(data_source, filter, actor, from, to);
                paged_movies = source_movies?
                                .OrderByDescending(m => m.ReleaseDate)
                                .Skip(currentPage * currentPageSize)
                                .Take(currentPageSize);

                totalMovies = (source_movies == null) ? 0 : source_movies.Count();              
            }
            else
            {
                source_movies = _moviesRepository.GetAllFromCache();
                paged_movies = source_movies?
                                .OrderByDescending(m => m.ReleaseDate)
                                .Skip(currentPage * currentPageSize)
                                .Take(currentPageSize)
                                .ToList();
                   
                totalMovies = (source_movies == null) ? 0 : source_movies.Count();
            }

            PaginationSet<IContent> pagedSet = new PaginationSet<IContent>()
            {
                Page = currentPage,
                TotalCount = totalMovies,
                TotalPages = (int)Math.Ceiling((decimal)totalMovies / currentPageSize),
                Items = paged_movies
            };

            JsonNetResult jsonResponse = new JsonNetResult()
            {
                Data = pagedSet,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

            return jsonResponse;

        }

        private bool IsAjax(ExceptionContext filterContext)
        {
            return filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
        protected override void OnException(ExceptionContext filterContext)
        {

            var currentController = (string)filterContext.RouteData.Values["controller"];
            var currentActionName = (string)filterContext.RouteData.Values["action"];

            logger.Error(filterContext.Exception);
            
            
            if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
            {
                return;
            }


            if (IsAjax(filterContext))
            {                
                filterContext.Result = new JsonResult()
                {
                    Data = filterContext.Exception.Message,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
            }
            else
            {               
                base.OnException(filterContext);

            }
          
        }



    }
}