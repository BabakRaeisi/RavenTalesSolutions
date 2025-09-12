using System.ComponentModel.DataAnnotations;

namespace CoreLayer.DTOs
{
    public record SegmentDto(
        [property: Range(0, int.MaxValue)]
        int StartChar,

        [property: Range(0, int.MaxValue)]
        int EndChar)
    {
        public SegmentDto() : this(0, 0)
        {
            // Default constructor
        }
    }
}