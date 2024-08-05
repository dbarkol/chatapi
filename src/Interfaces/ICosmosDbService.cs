using ChatApi.Models;

namespace ChatApi.Interfaces
{
    public interface ICosmosDbService
    {
        Task<List<Session>> GetSessionsAsync();

        Task<List<ChatMessage>> GetSessionMessagesAsync(string sessionId);

        Task<Session> GetSessionAsync(string id);

        Task<Session> InsertSessionAsync(Session session);

        Task<ChatMessage> InsertMessageAsync(ChatMessage message);

    }
}