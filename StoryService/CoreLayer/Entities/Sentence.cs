using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreLayer.Entities
{
    public class Sentence
    {
        public string Id { get; set; } = "";    // e.g., "s1"
        
        // Add for EF Core relationship
        [ForeignKey("StoryId")]
        public Guid StoryId { get; set; }
        
        public int StartChar { get; set; }      // 0-based, inclusive
        public int EndChar { get; set; }        // 0-based, exclusive
        public string Text { get; set; } = "";  // exact slice from Content
    }
}
