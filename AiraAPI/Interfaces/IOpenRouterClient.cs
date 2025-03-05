using AiraAPI.Models;

namespace AiraAPI.Interfaces
{
    public interface IOpenRouterClient
    {
        public IAsyncEnumerable<Message> GenerateStreamingMessageAsync(Message client_message);

        public Task<string> GenerateMessageAsync(Message client_message);
    }
}
