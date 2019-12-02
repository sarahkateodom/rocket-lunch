using RocketLunch.domain.enumerations;

namespace RocketLunch.domain.dtos
{
    public class SearchOptions
    {
        public MealTime Meal { get; set; }
        public Category Category { get; set; }

        public string Location {get; set; }
    }
}