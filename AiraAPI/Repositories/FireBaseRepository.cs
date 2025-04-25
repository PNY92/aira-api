using AiraAPI.Interfaces;
using AiraAPI.Models;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace AiraAPI.Repositories
{
    public class FireBaseRepository : IFireBaseRepository
    {

        private IFirebaseConfig _config;
        private IFirebaseClient _client;

        public FireBaseRepository(FirebaseConfig fireBaseConfig)
        {
            _config = fireBaseConfig;
            _client = new FirebaseClient(_config);
        }

        public async Task DeleteUserAsync()
        {
            await _client.DeleteAsync("user");

        }

        public async Task<User> GetUserAsync()
        {
            FirebaseResponse response = await _client.GetAsync("user/U01");
            User user = response.ResultAs<User>();
            return user;
        }

        public async Task SetUserAsync()
        {

        }

        public async Task UpdateUserAsync()
        {
            throw new NotImplementedException();
        }
    }
}
