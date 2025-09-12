using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreLayer.Entities
{
    public class Unit
    {
        public string Id { get; set; } = "";          // e.g., "u1"
        
        // Add for EF Core relationship
        [ForeignKey("StoryId")]
        public Guid StoryId { get; set; }
        
        public string SentenceId { get; set; } = "";  // links to Sentence.Id
        public bool IsDiscontinuous { get; set; }     // true if multiple segments
        public List<Segment> Segments { get; set; } = new(); // 1..N ranges in Content
        public List<string> Pieces { get; set; } = new();    // exact substrings for each segment
    }
}
