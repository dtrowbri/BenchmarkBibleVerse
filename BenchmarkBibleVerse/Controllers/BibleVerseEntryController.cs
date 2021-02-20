using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BenchmarkBibleVerse.Models;
using BenchmarkBibleVerse.Service.Business;

namespace BenchmarkBibleVerse.Controllers
{
    public class BibleVerseEntryController : Controller
    {
        // GET: BibleVerseEntry
        public ActionResult Index()
        {
            return View("CreateVerse");
        }

        [HttpPost]
        public ActionResult CreateVerse(BibleVerseModel verse)
        {
            if(ModelState.IsValid){
                BibleVerseService service = new BibleVerseService();
                service.AddVerse(verse);
                ModelState.Clear();
            }
            
            return View("CreateVerse");

        }


        public ActionResult SearchVerse()
        {
            return View("SearchVerse");
        }

        [HttpGet]
        public ActionResult GetVerse(BibleVerseModel verse)
        {
            BibleVerseModel m = verse;
            BibleVerseModel verse2 = new BibleVerseModel() {
                Testament = "new",
                ChapterSelect = 2,
                BookSelection = "John",
                VerseNumber = 78,
                VerseString = "And I shall walk through the valley of death with no fear."
            };

            return View("SearchVerse", verse2);
            
        }
        
    }
}