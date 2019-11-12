using System;
using System.Collections.Generic;
using System.Linq;
using RocketLunch.domain.dtos;

namespace RocketLunch.domain.utilities
{
    public class RestaurantCash
    {
        private static Dictionary<string, List<object>> _sessionData;

        private static Dictionary<string, DateTime> TimeStamps { get; set; }

        private static object locker = new Object();

        static RestaurantCash()
        {
            TimeStamps = new Dictionary<string, DateTime>();
            _sessionData = new Dictionary<string, List<Object>>();
        }

        public static List<RestaurantDto> GetRestaurantList(Guid sessionId)
        {
            Cleanup();
            string id = sessionId.ToString() + "_searchsession";
            DateTime expireTimeStamp;
            if (!TimeStamps.TryGetValue(id, out expireTimeStamp)) return null;
            TimeSpan timeSinceLastSet = DateTime.UtcNow - expireTimeStamp;
            if (timeSinceLastSet.TotalMinutes >= 5) return null;
            List<object> currentValue;
            if (!_sessionData.TryGetValue(id, out currentValue)) return null;
            return currentValue.Select(x => (RestaurantDto)x).ToList();
        }

        public static void SetRestaurantList(Guid sessionId, List<RestaurantDto> cash)
        {
            Cleanup();
            string id = sessionId.ToString() + "_searchsession";
            if (_sessionData.ContainsKey(id)) _sessionData.Remove(id);
            _sessionData.Add(id, cash.Select(x => (object)x).ToList());
            TimeStamps.TryAdd(id, DateTime.UtcNow);
        }

        public static void AddSeenOption(string sessionId, string option)
        {
            Cleanup();
            List<object> options;
            if (!_sessionData.TryGetValue(sessionId, out options))
            {
                options = new List<object>();
                _sessionData.Add(sessionId, options);
            }
            options.Add(option);
            TimeStamps.TryAdd(sessionId, DateTime.UtcNow);
        }

        public static List<string> GetSeenOptions(string sessionId)
        {
            List<object> options;
            if (!_sessionData.TryGetValue(sessionId, out options)) options = new List<object>();
            return options.Select(x => (string)x).ToList();
        }

        public static void CreateUpdateUserSession(Guid sessionId, List<int> userIds)
        {
            Cleanup();
            string id = sessionId.ToString() + "_session";
            List<object> currentValue;
            List<object> newValue = userIds.Select(x => (object)x.ToString()).ToList();
            if (!_sessionData.TryGetValue(id, out currentValue))
            {
                _sessionData.Add(id, newValue);
                TimeStamps.TryAdd(id, DateTime.UtcNow);
            }
            else
            {
                _sessionData[id] = newValue;
                TimeStamps.Remove(id);
                TimeStamps.TryAdd(id, DateTime.UtcNow);
            }
        }

        public static List<int> GetUserSession(string sessionId)
        {
            string id = sessionId.ToString() + "_session";
            List<object> users;
            _sessionData.TryGetValue(id, out users);
            if (users == null) return null;
            List<int> userIds = users.Select(x => int.Parse((string)x)).ToList();
            return userIds;
        }


        private static void Cleanup()
        {
            lock (locker)
            {
                DateTime now = DateTime.UtcNow;
                var keys = TimeStamps.Where(x => (now - x.Value).TotalMinutes > 60).Select(x => x.Key).Where(x => !String.IsNullOrEmpty(x));
                foreach (string key in keys)
                {
                    _sessionData.Remove(key);
                    TimeStamps.Remove(key);
                }
            }
        }
    }
}