using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreLayer.DTOs
{
    public record UnitDto(
        [property: Required] string Id,
        [property: Required] string SentenceId,
        bool IsDiscontinuous,
        [property: Required] IReadOnlyList<SegmentDto> Segments,
        [property: Required] IReadOnlyList<string> Pieces
    )
    {
        public UnitDto() : this("", "", false, new List<SegmentDto>(), new List<string>()) { }
    }
}
