using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BenchmarkBibleVerse.Models
{
    public class BibleVerseModel
    {
        public string Testament { get; set; }

        public string BookSelection { get; set; }
        
        public int ChapterSelect { get; set; }
        public int VerseNumber { get; set; }
        public string VerseString { get; set; }
    }
}