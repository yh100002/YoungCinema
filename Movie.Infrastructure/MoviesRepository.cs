using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NLog;

using Movie.Core;
using Movie.Core.Enums;
using Movie.Core.Interfaces;
using Movie.Infrastructure.Configuration;
using Movie.Infrastructure.Handlers;
using Movie.Infrastructure.ParameterFactory;

namespace Movie.Infrastructure
{
    public class MoviesRepository : IMoviesRepository, IDisposable
    {

        Logger logger = LogManager.GetCurrentClassLogger();
        public IEnumerable<IContent> SearchMovie(string strType,string filter,string actor,int from, int to)
        {
            try
            {
                DataSourceType sourceType = EnumConverter.Parse(strType);
                ParamCreator ParamFactory = new ParamCreator();
                Param ParamObj = ParamFactory.Create(sourceType, filter, actor, from, to);
                return ParamObj.Search();
            }
            catch(Exception ex)
            {
                logger.Error(ex);
            }
            return null;                     
        }


        public IEnumerable<IContent> GetAllFromCache()
        {
            try
            {
                DataSourceType sourceType = EnumConverter.Parse("cache");
                ParamCreator ParamFactory = new ParamCreator();
                Param ParamObj = ParamFactory.Create(sourceType, "", "", 0, 0);
                return ParamObj.Search();                
            }
            catch(Exception ex)
            {
                logger.Error(ex);
            }
            return null;         
        }

        public void ClearCache()
        {
            try
            {
                DataSourceType sourceType = EnumConverter.Parse("cache");
                ParamCreator ParamFactory = new ParamCreator();
                Param ParamObj = ParamFactory.Create(sourceType, "", "", 0, 0);
                ParamObj.Clear();
                
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }          
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {                    
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IEnumerable<IContent> SearchMovie(string filter)
        {
            throw new NotImplementedException();
        }

        public void CloseConnection()
        {
            throw new NotImplementedException();
        }
    }
}
