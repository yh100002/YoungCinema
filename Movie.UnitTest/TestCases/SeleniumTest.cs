using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Protractor;

using Movie.Infrastructure;

namespace Movie.UnitTest
{
    [TestClass]
    public class SeleniumTest
    {
        public static string BaseUrl = "http://localhost:8240/";

        // the max. time to wait before timing out.
        public const int TimeOut = 200;

        [TestMethod]
        public void SimpleYoutubeSearchTest()
        {
            ContentsRepository repo = new ContentsRepository();
            repo.ClearCache();

            var webdriver = new ChromeDriver();

            webdriver.Manage().Timeouts().AsynchronousJavaScript = new System.TimeSpan(TimeOut);
            var ngDriver = new NgWebDriver(webdriver);
            ngDriver.Url = BaseUrl;
            ngDriver.Manage().Window.Maximize();
                     
            SelectElement selectBox = new SelectElement(ngDriver.FindElement(By.Id("selectDataSource")));
            foreach(var op in selectBox.Options)
            {
                if(op.Text == "YouTube")
                {
                    op.Click();
                    break;
                }
            }

            ngDriver.FindElement(By.Id("inputSearchMovies")).SendKeys("007");
            ngDriver.FindElement(By.Id("btnSearch")).Click();

            try
            {
                var title = ngDriver.FindElement(NgBy.Binding("movie.Title"))?.Text;
                Assert.IsTrue(!string.IsNullOrEmpty(title));
            }
            catch (Exception)
            {
                Assert.Fail();
            }

        }

        [TestMethod]
        public void SimpleTMDBSearchTest() //FREQUENT TIME OUT EXCEPTION BECAUSE OF SLOW WEB API LOADING
        {
            ContentsRepository repo = new ContentsRepository();
            repo.ClearCache();
            var webdriver = new ChromeDriver();

            webdriver.Manage().Timeouts().AsynchronousJavaScript = new System.TimeSpan(TimeOut);
            var ngDriver = new NgWebDriver(webdriver);
            ngDriver.Url = BaseUrl;
            ngDriver.Manage().Window.Maximize();

            SelectElement selectBox = new SelectElement(ngDriver.FindElement(By.Id("selectDataSource")));
            foreach (var op in selectBox.Options)
            {
                if (op.Text == "TMDB")
                {
                    op.Click();
                    break;
                }
            }

            ngDriver.FindElement(NgBy.Model("dtFrom")).Clear();
            ngDriver.FindElement(NgBy.Model("dtFrom")).SendKeys("2000");

            ngDriver.FindElement(NgBy.Model("dtTo")).Clear();
            ngDriver.FindElement(NgBy.Model("dtTo")).SendKeys("2017");

            ngDriver.FindElement(By.Id("inputSearchMovies")).SendKeys("007");
            ngDriver.FindElement(By.Id("btnSearch")).Click();

            try
            {
                var title = ngDriver.FindElement(NgBy.Binding("movie.Title"))?.Text;
                Assert.IsTrue(!string.IsNullOrEmpty(title));
            }
            catch (Exception)
            {
                Assert.Fail();
            }
            finally
            {
                webdriver.Close();
            }

        }

        [TestMethod]
        public void SimpleCacheSearchTest() //FREQUENT TIME OUT EXCEPTION BECAUSE OF SLOW WEB API LOADING
        {

            var webdriver = new ChromeDriver();

            webdriver.Manage().Timeouts().AsynchronousJavaScript = new System.TimeSpan(TimeOut);
            var ngDriver = new NgWebDriver(webdriver);
            ngDriver.Url = BaseUrl;
            ngDriver.Manage().Window.Maximize();

            SelectElement selectBox = new SelectElement(ngDriver.FindElement(By.Id("selectDataSource")));
            foreach (var op in selectBox.Options)
            {
                if (op.Text == "Cache")
                {
                    op.Click();
                    break;
                }
            }

            ngDriver.FindElement(NgBy.Model("dtFrom")).Clear();
            ngDriver.FindElement(NgBy.Model("dtFrom")).SendKeys("2000");

            ngDriver.FindElement(NgBy.Model("dtTo")).Clear();
            ngDriver.FindElement(NgBy.Model("dtTo")).SendKeys("2016");

            ngDriver.FindElement(By.Id("inputSearchMovies")).SendKeys("007");
            ngDriver.FindElement(By.Id("btnSearch")).Click();

            try
            {
                var title = ngDriver.FindElement(NgBy.Binding("movie.Title"))?.Text;
                Assert.IsTrue(!string.IsNullOrEmpty(title));
            }
            catch(Exception)
            {
                Assert.Fail();
            }

        }

        [TestMethod]
        public void SimpleIntegratedSearchTest() 
        {
            ContentsRepository repo = new ContentsRepository();
            repo.ClearCache();
            var webdriver = new ChromeDriver();

            webdriver.Manage().Timeouts().AsynchronousJavaScript = new System.TimeSpan(TimeOut);
            var ngDriver = new NgWebDriver(webdriver);
            ngDriver.Url = BaseUrl;
            ngDriver.Manage().Window.Maximize();

            SelectElement selectBox = new SelectElement(ngDriver.FindElement(By.Id("selectDataSource")));
            foreach (var op in selectBox.Options)
            {
                if (op.Text == "Integrated")
                {
                    op.Click();
                    break;
                }
            }

            ngDriver.FindElement(NgBy.Model("dtFrom")).Clear();
            ngDriver.FindElement(NgBy.Model("dtFrom")).SendKeys("2000");

            ngDriver.FindElement(NgBy.Model("dtTo")).Clear();
            ngDriver.FindElement(NgBy.Model("dtTo")).SendKeys("2016");

            ngDriver.FindElement(By.Id("inputSearchMovies")).SendKeys("terminator 2");
            ngDriver.FindElement(By.Id("inputSearchActor")).SendKeys("arnold");
            ngDriver.FindElement(By.Id("btnSearch")).Click();

            try
            {
                var title = ngDriver.FindElement(NgBy.Binding("movie.Title"))?.Text;
                Assert.IsTrue(!string.IsNullOrEmpty(title));
            }
            catch (Exception)
            {
                Assert.Fail();
            }

        }
    }
}
