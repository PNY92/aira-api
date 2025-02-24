using AiraAPI.Models;

namespace AiraAPI.Interfaces
{
    public interface IOpenRouterRepository
    {

        public void SetAPIKey(string api_key);

        public Task<Message> GenerateMessageAsync(Message client_message);


    }
}
