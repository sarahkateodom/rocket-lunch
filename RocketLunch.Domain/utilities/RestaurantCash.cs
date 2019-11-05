using System;
using System.Collections.Generic;
using System.Linq;
using RocketLunch.domain.dtos;

namespace RocketLunch.domain.utilities
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

        public static void CreateUpdateUserSession(Guid sessionId, List<int> userIds)
        {
            Cleanup();
            string id = sessionId.ToString() + "_session";
            List<string> currentValue;
            List<string> newValue = userIds.Select(x => x.ToString()).ToList();
            if (!_sessionData.TryGetValue(id, out currentValue))
            {
                _sessionData.Add(id, newValue);
                TimeStamps.TryAdd(id, DateTime.UtcNow);
            }
            {
                _sessionData[id] = newValue;
                TimeStamps.Remove(id);
                TimeStamps.TryAdd(id, DateTime.UtcNow);
            }
        }

        public static List<int> GetUserSession(string sessionId)
        {
            string id = sessionId.ToString() + "_session";
            List<string> users;
            _sessionData.TryGetValue(id, out users);
            if (users == null) return null;
            List<int> userIds = users.Select(x => int.Parse(x)).ToList();
            return userIds;
        }

        private static Dictionary<string, List<string>> _sessionData;

        private static Dictionary<string, DateTime> TimeStamps { get; set; }

        private static object locker = new Object();

        private static void Cleanup()
        {
            lock(locker)
            {
                DateTime now = DateTime.UtcNow;
                var keys = TimeStamps.Where(x => (now - x.Value).TotalMinutes > 60 && x.Key != "mainCache").Select(x => x.Key).Where(x => !String.IsNullOrEmpty(x));
                foreach(string key in keys)
                {
                    _sessionData.Remove(key);
                    TimeStamps.Remove(key);
                }
            }
        }
    }
}