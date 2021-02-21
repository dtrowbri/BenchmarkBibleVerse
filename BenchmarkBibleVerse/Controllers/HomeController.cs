using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BenchmarkBibleVerse.Service.Utility;

namespace BenchmarkBibleVerse.Controllers
{
    public class HomeController : Controller
    {

        /*
         * Creates logger and receives it using dependency injection
         */
        private ILogger logger;
        
        public HomeController(ILogger logger)
        {
            this.logger = logger;
        }
        // GET: Home
        public ActionResult Index()
        {
            logger.Info("Action Index in Home controller has been invoked.");
            return View("Home");
        }
    }
}