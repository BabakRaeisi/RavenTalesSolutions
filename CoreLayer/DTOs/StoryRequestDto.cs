using CoreLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.DTOs
{
    public class StoryRequestDto
    {
        [Required]
        [EnumDataType(typeof(LanguageLevel))]
        public LanguageLevel LanguageLevel { get; set; }

        [Required]
        [EnumDataType(typeof(TargetLanguage))]
        public TargetLanguage TargetLanguage { get; set; }

        [StringLength(255)]
        public string? Topic { get; set; }
    }
}
