using System.Collections.Generic;

namespace RocketLunch.domain.dtos
{
    public class YelpResultDto
    {
        public IEnumerable<RestaurantDto> Businesses { get; set; }
        public int Total { get; set; }
    }
}