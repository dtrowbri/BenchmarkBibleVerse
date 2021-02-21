using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BenchmarkBibleVerse.Service.Data;
using BenchmarkBibleVerse.Models;
using BenchmarkBibleVerse.Service.Utility;

namespace BenchmarkBibleVerse.Service.Business
{
    public class BibleVerseService
    {
        /*
         * Gets logger from the view.
         */
        private readonly ILogger logger;
        public BibleVerseService(ILogger logger)
        {
            this.logger = logger;
        }
        public bool AddVerse(BibleVerseModel bibleVerse)
        {
            BibleVerseDAO dao = new BibleVerseDAO(logger);
            return dao.AddVerse(bibleVerse);
        }

        public BibleVerseModel GetVerse(BibleVerseModel verse)
        {
            BibleVerseDAO dao = new BibleVerseDAO(logger);
            return dao.GetVerse(verse);
        }
    }
}