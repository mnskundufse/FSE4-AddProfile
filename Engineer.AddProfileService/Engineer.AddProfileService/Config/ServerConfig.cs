using Engineer.AddProfileService.Config;

namespace Engineer.AddProfileService.Models
{
    public class ServerConfig
    {
        public MongoDbConfig MongoDB { get; set; } = new MongoDbConfig();
    }
}