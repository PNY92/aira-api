using AiraAPI.Models;

namespace AiraAPI.Interfaces
{
    public interface IOpenRouterRepository
    {

        public void SetAPIKey(string api_key);

        public IAsyncEnumerable<Message> GenerateMessageAsync(Message client_message);


    }
}
