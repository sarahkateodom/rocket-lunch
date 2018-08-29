using System.Collections.Generic;

namespace makelunch.domain.dtos
{
    public class YelpResultDto
    {
        public IEnumerable<RestaurantDto> Businesses { get; set; }
        public int Total { get; set; }
    }
}