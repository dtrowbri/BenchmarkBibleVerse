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
        /*
         * Creates logger and receives it using dependency injection
         */
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
            /*
             * Checks if the model passed and returns validation errors if invalid
             */
            if(ModelState.IsValid){
                /*
                 * Tries to write verse to database.
                 * Clears model before returning view
                 */
                logger.Info("Bible verse with parameters " + new JavaScriptSerializer().Serialize(verse) + " is being created. BibleVerseModel ModelState is valid.");
                BibleVerseService service = new BibleVerseService(logger);
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
            logger.Info("Action SearchVerse from BibleVerseEntry controller has been invoked.");
            return View("SearchVerse");
        }

        [HttpGet]
        public ActionResult GetVerse(BibleVerseModel verse)
        {
            /*
             * Tries to retrieve verse from database
             */
            logger.Info("Search request for verse " + new JavaScriptSerializer().Serialize(verse) + " executing.");
            BibleVerseService service = new BibleVerseService(logger);
            try
            {
                verse = service.GetVerse(verse);
            } catch (Exception ex)
            {
                logger.Error("Exception occured searching for verse " + new JavaScriptSerializer().Serialize(verse) + "\nException: " + ex.Message);
            }

            /*
             * Creates different logs if the verse was successfully retrieved from the database
             */
            if(verse.VerseString == null || verse.VerseString == "")
            {
                logger.Warning("No verse was found for " + new JavaScriptSerializer().Serialize(verse));
            } else
            {
                logger.Info("Search request completed successfully.");
            }
            return View("SearchVerse", verse);
            
        }
        
    }
}