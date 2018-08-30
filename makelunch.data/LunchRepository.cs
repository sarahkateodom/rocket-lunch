using System;
using System.Threading.Tasks;
using makelunch.domain.contracts;

namespace makelunch.data
{
    public class LunchRepository : IRepository
    {
        private LunchContext _lunchContext;
        public LunchRepository(LunchContext lunchContext)
        {
            _lunchContext = lunchContext ?? throw new ArgumentNullException("lunchContext");
        }

        public Task CreateUserAsync(string name, string avatarUrl)
        {
            throw new System.NotImplementedException();
        }
    }
}