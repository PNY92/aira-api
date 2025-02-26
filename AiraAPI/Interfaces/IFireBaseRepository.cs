using AiraAPI.Models;

namespace AiraAPI.Interfaces
{
    public interface IFireBaseRepository
    {
        public Task SetUserAsync();

        public Task<User> GetUserAsync();

        public Task UpdateUserAsync();

        public Task DeleteUserAsync();
    }
}
