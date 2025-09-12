using CoreLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.DTOs
{
        // Plan:
        // - Convert class to positional record with primary constructor (LanguageLevel, TargetLanguage, Topic).
        // - Keep validation by applying DataAnnotations to properties using [property:] target.
        // - Add parameterless constructor that initializes with default values.

        public record class StoryRequestDto(
            [property: System.ComponentModel.DataAnnotations.Required]
            [property: System.ComponentModel.DataAnnotations.EnumDataType(typeof(CoreLayer.Enums.LanguageLevel))]
            CoreLayer.Enums.LanguageLevel LanguageLevel,

            [property: System.ComponentModel.DataAnnotations.Required]
            [property: System.ComponentModel.DataAnnotations.EnumDataType(typeof(CoreLayer.Enums.TargetLanguage))]
            CoreLayer.Enums.TargetLanguage TargetLanguage,

            [property: System.ComponentModel.DataAnnotations.StringLength(255)]
            string? Topic)
        {
            public StoryRequestDto() : this(default, default, default)
            {
                // Default constructor initializes with default values
            }
        }
}
