using CoreLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreLayer.Helpers
{
    public static class StoryValidator
    {
        /// <summary>
        /// Ensures sentences and units have valid and unique IDs
        /// </summary>
        public static void EnsureValidIds(Story story)
        {
            // Reset ID counters
            var sentenceIdCounter = 1;
            var unitIdCounter = 1;
            
            // Create a dictionary to track old to new sentence IDs for remapping
            var sentenceIdMap = new Dictionary<string, string>();
            
            // Ensure each Sentence has a unique ID
            foreach (var sentence in story.Sentences)
            {
                var oldId = sentence.Id;
                var newId = $"s{sentenceIdCounter++}";
                
                // Store mapping of old ID to new ID
                if (!string.IsNullOrEmpty(oldId))
                {
                    sentenceIdMap[oldId] = newId;
                }
                
                sentence.Id = newId;
            }
            
            // Ensure each Unit has a unique ID and valid SentenceId
            foreach (var unit in story.Units)
            {
                // Update unit ID
                unit.Id = $"u{unitIdCounter++}";
                
                // Update unit's SentenceId reference if it was remapped
                if (sentenceIdMap.TryGetValue(unit.SentenceId, out var newSentenceId))
                {
                    unit.SentenceId = newSentenceId;
                }
            }
        }
        
        /// <summary>
        /// Validates and corrects the structural integrity of a story
        /// </summary>
        public static void ValidateStoryStructure(Story story)
        {
            // Create a set of valid sentence IDs
            var validSentenceIds = new HashSet<string>(story.Sentences.Select(s => s.Id));
            
            // Validate that each unit references a valid sentence
            foreach (var unit in story.Units)
            {
                if (!validSentenceIds.Contains(unit.SentenceId))
                {
                    throw new InvalidOperationException($"Unit {unit.Id} references non-existent SentenceId {unit.SentenceId}");
                }
                
                // Validate that segments and pieces have matching counts
                if (unit.Segments.Count != unit.Pieces.Count)
                {
                    throw new InvalidOperationException($"Unit {unit.Id} has mismatched segment and piece counts");
                }
                
                // Validate that IsDiscontinuous flag is set correctly
                if ((unit.Segments.Count > 1) != unit.IsDiscontinuous)
                {
                    unit.IsDiscontinuous = unit.Segments.Count > 1;
                }
            }
            
            // Validate sentence text against content boundaries
            foreach (var sentence in story.Sentences)
            {
                if (sentence.StartChar < 0 || sentence.EndChar > story.Content.Length || sentence.StartChar >= sentence.EndChar)
                {
                    throw new InvalidOperationException($"Sentence {sentence.Id} has invalid character boundaries");
                }
                
                // Verify text matches content slice
                var expectedText = story.Content.Substring(sentence.StartChar, sentence.EndChar - sentence.StartChar);
                if (sentence.Text != expectedText)
                {
                    sentence.Text = expectedText;
                }
            }
        }
    }
}