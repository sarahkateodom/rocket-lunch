
using System.Collections.Generic;

namespace RocketLunch.domain.dtos
{
    public class RestaurantDto
    {
        public string Rating { get; set; }
        public string Image_url { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int Review_count { get; set; }
        public string Id { get; set; }
        public List<CategoryDto> Categories { get; set; }
    }
}