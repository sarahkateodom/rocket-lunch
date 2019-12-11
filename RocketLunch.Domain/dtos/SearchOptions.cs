using System.Collections.Generic;
using RocketLunch.domain.enumerations;

namespace RocketLunch.domain.dtos
{
    public class SearchOptions
    {
        public SearchOptions()
        {
            this.UserIds = new List<int>();
        }
        
        public MealTime Meal { get; set; }
        public IEnumerable<int> UserIds { get; set; }
        public string Zip { get; set; }
        // public Category Category { get; set; }
    }
}