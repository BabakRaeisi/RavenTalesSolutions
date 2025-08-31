using CoreLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Entities
{
    public class UserPreferences
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        public LanguageLevel LastUsedLanguageLevel { get; set; }

        [Required]
        public TargetLanguage LastUsedTargetLanguage { get; set; }

        [Required]
        public TargetLanguage PreferredTranslationLanguage { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        // Navigation property
        public virtual ICollection<Story> Stories { get; set; } = new List<Story>();
    }
}
