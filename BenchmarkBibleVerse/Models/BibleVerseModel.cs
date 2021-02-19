using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BenchmarkBibleVerse.Models
{
    public class BibleVerseModel
    {
        [Required]
        [StringLength(3, MinimumLength =3)]
        public string Testament { get; set; }

        [Required]
        [StringLength(50)]
        public string BookSelection { get; set; }
        
        [Required]
        [Range(1,1000)]
        public int ChapterSelect { get; set; }
        
        [Required]
        [Range(1,1000)]
        public int VerseNumber { get; set; }
        
        [Required]
        [MinLength(1)]
        public string VerseString { get; set; }
    }
}