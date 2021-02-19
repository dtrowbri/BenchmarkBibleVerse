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
            return View("CreateView");
        }

        public ActionResult CreateVerse(BibleVerseModel verse)
        {
            if(ModelState.IsValid){
                BibleVerseService service = new BibleVerseService();
                service.AddVerse(verse);
                ModelState.Clear();
            }
            
            return View("CreateView");

        }
    }
}