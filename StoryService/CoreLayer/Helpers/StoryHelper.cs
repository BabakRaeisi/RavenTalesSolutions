using CoreLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreLayer.Helpers
{
    public static class StoryHelper
    {
        /// <summary>
        /// Filters a collection of stories to exclude those already seen by the user
        /// </summary>
        public static IEnumerable<Story> FilterUnseenStories(IEnumerable<Story> stories, UserPreferences userPreferences)
        {
            var seenStoryIds = userPreferences?.SeenStoryIds ?? new List<Guid>();
            return stories.Where(s => !seenStoryIds.Contains(s.Id));
        }
  
    }
}