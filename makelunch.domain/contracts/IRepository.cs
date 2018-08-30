using System.Threading.Tasks;

namespace makelunch.domain.contracts
{
    public interface IRepository
    {
        Task<int> CreateUserAsync(string name, string avatarUrl);
    }
}