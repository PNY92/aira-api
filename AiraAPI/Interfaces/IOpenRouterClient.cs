using AiraAPI.Models;

namespace AiraAPI.Interfaces
{
    public interface IOpenRouterClient
    {
        public IAsyncEnumerable<Message> GenerateMessageAsync(Message client_message);


    }
}
