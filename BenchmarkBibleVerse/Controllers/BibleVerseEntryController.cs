using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BenchmarkBibleVerse.Models;
using BenchmarkBibleVerse.Service.Business;
using BenchmarkBibleVerse.Service.Utility;

namespace BenchmarkBibleVerse.Controllers
{
    public class BibleVerseEntryController : Controller
    {
        private readonly ILogger logger;
        public BibleVerseEntryController(ILogger logger)
        {
            this.logger = logger;
        }

        // GET: BibleVerseEntry
        public ActionResult Index()
        {
            logger.Info("Action Index invoked from the BibleVerseEntry controller.");
            return View("CreateVerse");
        }

        [HttpPost]
        public ActionResult CreateVerse(BibleVerseModel verse)
        {
            
            if(ModelState.IsValid){
                logger.Info("Bible verse with parameters " + new JavaScriptSerializer().Serialize(verse) + " is being created. BibleVerseModel ModelState is valid.");
                BibleVerseService service = new BibleVerseService();
                try
                {
                    service.AddVerse(verse);
                    logger.Info("Bible verse with parameters " + new JavaScriptSerializer().Serialize(verse) + " has been successfully saved to the database.");
                }
                catch (Exception ex)
                {
                    logger.Error("Exception has occured. Exception " + ex.Message);
                }
                ModelState.Clear();
            } else
            {
                logger.Warning("Bible verse with parameters " + new JavaScriptSerializer().Serialize(verse) + " is being not being created. BibleVerseModel ModelState is invalid");
            }
            
            return View("CreateVerse");

        }


        public ActionResult SearchVerse()
        {
            logger.Info("User has called")
            return View("Action SearchVerse invoked from the BibleVerseEntry controller.");
        }

        [HttpGet]
        public ActionResult GetVerse(BibleVerseModel verse)
        {
            logger.Info("Search request for verse " + new JavaScriptSerializer().Serialize(verse) + " executing.");
            BibleVerseService service = new BibleVerseService();
            try
            {
                verse = service.GetVerse(verse);
            } catch (Exception ex)
            {
                logger.Error("Exception occured searching for verse " + new JavaScriptSerializer().Serialize(verse) + "\nException: " + ex.Message);
            }

            if(verse.VerseString == null || verse.VerseString == "")
            {
                logger.Warning("No verse was found for " + new JavaScriptSerializer().Serialize(verse));
            }
            return View("SearchVerse", verse);
            
        }
        
    }
}