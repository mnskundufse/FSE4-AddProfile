using Engineer.AddProfileService.Model;
using System.Threading.Tasks;

namespace Engineer.AddProfileService.Business.Contracts
{
    public interface IAddProfileBusiness
    {
        Task<ApiResponse> AddUserProfileBusiness(UserProfile userProfile);
    }
}
