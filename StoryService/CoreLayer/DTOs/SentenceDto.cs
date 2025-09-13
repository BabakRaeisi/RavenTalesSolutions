using System.ComponentModel.DataAnnotations;

namespace CoreLayer.DTOs
{
    public record SentenceDto(
        [property: Required] string Id,
        [property: Range(0, int.MaxValue)] int StartChar,
        [property: Range(0, int.MaxValue)] int EndChar,
        [property: Required] string Text
    )
    {
        public SentenceDto() : this("", 0, 0, "") { }
    }
}
