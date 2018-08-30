using System.Threading.Tasks;

namespace makelunch.domain.contracts
{
    public interface IRepository
    {
        Task CreateUserAsync(string name, string avatarUrl);
    }
}