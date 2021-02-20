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
        public bool AddVerse(BibleVerseModel bibleVerse)
        {
            BibleVerseDAO dao = new BibleVerseDAO();
            return dao.AddVerse(bibleVerse);
        }

        public BibleVerseModel GetVerse(BibleVerseModel verse)
        {
            BibleVerseDAO dao = new BibleVerseDAO();
            return dao.GetVerse(verse);
        }
    }
}