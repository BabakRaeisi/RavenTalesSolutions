using CoreLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreLayer.Entities
{
    public class Story
    {
        public Guid Id { get; set; }
 
        [Required]
        [StringLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public LanguageLevel LanguageLevel { get; set; }

        [Required]
        public TargetLanguage TargetLanguage { get; set; }

        [StringLength(100)]
        public string Genre { get; set; } = string.Empty; // Added genre property

        public DateTime CreatedAt { get; set; }



        public List<Sentence> Sentences { get; set; } = new();
        public List<Unit> Units { get; set; } = new();

   
    }
}
