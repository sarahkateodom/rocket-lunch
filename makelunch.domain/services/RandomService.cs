using System;
using makelunch.domain.contracts;

namespace makelunch.domain.services
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