using Engineer.AddProfileService.Model;
using MongoDB.Driver;

namespace Engineer.AddProfileService.Repository.Contracts
{
    public interface IAddProfileContext
    {
        IMongoCollection<UserProfile> UserProfile { get; }
    }
}
