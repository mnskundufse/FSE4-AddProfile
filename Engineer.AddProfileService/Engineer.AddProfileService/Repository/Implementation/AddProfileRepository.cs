using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Engineer.AddProfileService.Model;
using Engineer.AddProfileService.Repository.Contracts;

namespace Engineer.AddProfileService.Repository.Implementation
{
    public class AddProfileRepository : IAddProfileRepository
    {
        private readonly IAddProfileContext _context;
        public AddProfileRepository(IAddProfileContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Insert User Profile (Repository)
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        public async Task AddUserProfileRepository(UserProfile userProfile)
        {
            await _context.UserProfile.InsertOneAsync(userProfile);
        }

        /// <summary>
        /// Search for next available user id
        /// </summary>
        /// <returns></returns>
        public async Task<long> GetNextUserId()
        {
            long currentLatestUserId = 0;
            var sort = Builders<UserProfile>.Sort.Descending("UserId");
            long recordCount = await _context.UserProfile.CountDocumentsAsync(new BsonDocument());
            if (recordCount > 0)
            {
                UserProfile latestUserProfile = _context.UserProfile.Find(u => u.UserId != 0).Sort(sort).First();
                currentLatestUserId = latestUserProfile.UserId;
            }
            
            return currentLatestUserId + 1;
        }
    }
}