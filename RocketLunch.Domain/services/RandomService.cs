using System;
using RocketLunch.domain.contracts;

namespace RocketLunch.domain.services
{
    public class RandomService : IChaos
    {
        private Random _random;
        public RandomService()
        {
            _random = new Random();
        }

        public int Next(int maxValue)
        {
            return _random.Next(maxValue);
        }
    }
}