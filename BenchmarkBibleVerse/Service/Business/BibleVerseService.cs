using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BenchmarkBibleVerse.Service.Data;
using BenchmarkBibleVerse.Models;

namespace BenchmarkBibleVerse.Service.Business
{
    public class BibleVerseService
    {
        public string AddVerse(BibleVerseModel bibleVerse)
        {
            BibleVerseDAO dao = new BibleVerseDAO();
            dao.AddVerse(bibleVerse);
            return "Completed test";
        }
    }
}