using System;
using System.Collections.Generic;
using makelunch.domain.dtos;

namespace makeLunch.domain.utilities
{
    public class RestaurantCash
    {
        private static List<RestaurantDto> _restaurantList;

        public static List<RestaurantDto> RestaurantList
        {
            get
            {
                TimeSpan timeSinceLastSet = DateTime.UtcNow - TimeStamp;
                if (timeSinceLastSet.TotalHours >= 24) return null;
                return _restaurantList;
            }
            set
            {
                _restaurantList = value;
                TimeStamp = DateTime.UtcNow;
            }
        }
        private static DateTime TimeStamp { get; set; }
    }
}