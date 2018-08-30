using System;
using System.Collections.Generic;
using System.Linq;
using makelunch.domain.dtos;

namespace makeLunch.domain.utilities
{
    public class RestaurantCash
    {
        static RestaurantCash()
        {
            TimeStamps = new Dictionary<string, DateTime>();
            _sessionData = new Dictionary<string, List<String>>();
        }

        private static List<RestaurantDto> _restaurantList;

        public static List<RestaurantDto> RestaurantList
        {
            get
            {
                DateTime mainTimeStamp;
                TimeStamps.TryGetValue("mainCache", out mainTimeStamp);
                TimeSpan timeSinceLastSet = DateTime.UtcNow - mainTimeStamp;
                if (timeSinceLastSet.TotalHours >= 24) return null;
                return _restaurantList;
            }
            set
            {
                _restaurantList = value;
                TimeStamps.Add("mainCache", DateTime.UtcNow);
            }
        }

        public static void AddSeenOption(string sessionId, string option)
        {
            Cleanup();
            List<string> options;
            if (!_sessionData.TryGetValue(sessionId, out options))
            {
                options = new List<string>();
                _sessionData.Add(sessionId, options);
            }
            options.Add(option);
            TimeStamps.TryAdd(sessionId, DateTime.UtcNow);
        }

        public static List<string> GetSeenOptions(string sessionId)
        {
            List<string> options;
            _sessionData.TryGetValue(sessionId, out options);
            return options;
        }

        private static Dictionary<string, List<string>> _sessionData;

        private static Dictionary<string, DateTime> TimeStamps { get; set; }

        private static object locker = new Object();

        private static void Cleanup()
        {
            lock(locker)
            {
                DateTime now = DateTime.UtcNow;
                var keys = TimeStamps.Where(x => (now - x.Value).TotalMinutes > 60 && x.Key != "mainCache").Select(x => x.Key);
                foreach(string key in keys)
                {
                    _sessionData.Remove(key);
                    TimeStamps.Remove(key);
                }
            }
        }
    }
}