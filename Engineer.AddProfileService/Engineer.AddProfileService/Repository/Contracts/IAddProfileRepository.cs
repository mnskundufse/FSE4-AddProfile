using Engineer.AddProfileService.Model;
using System.Threading.Tasks;

namespace Engineer.AddProfileService.Repository.Contracts
{
    public interface IAddProfileRepository
    {
        Task AddUserProfileRepository(UserProfile userProfile);
        Task<long> GetNextUserId();
    }
}
