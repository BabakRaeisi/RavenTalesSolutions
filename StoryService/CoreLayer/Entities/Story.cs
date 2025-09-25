using RavenTales.Shared;
 
using System.ComponentModel.DataAnnotations;
 

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
        public  Language StoryLanguage { get; set; }

        [StringLength(100)]
        public string Genre { get; set; } = string.Empty; // Added genre property

        public DateTime CreatedAt { get; set; }



        public List<Sentence> Sentences { get; set; } = new();
        public List<Unit> Units { get; set; } = new();

   
    }
}
