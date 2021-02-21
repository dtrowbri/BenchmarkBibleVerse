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
        [Display(Name ="Testament:")]
        [StringLength(3, MinimumLength =3)]
        public string Testament { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Book:")]
        public string BookSelection { get; set; }
        
        [Required]
        [Range(1,1000)]
        [Display(Name = "Chapter:")]
        public int ChapterSelect { get; set; }
        
        [Required]
        [Range(1,1000)]
        [Display(Name = "Verse #:")]
        public int VerseNumber { get; set; }
        
        [Required]
        [MinLength(1)]
        [Display(Name = "Verse:")]
        public string VerseString { get; set; }
    }
}