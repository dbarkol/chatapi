using Microsoft.Azure.Cosmos;
using ChatApi.Models;
using ChatApi.Interfaces;

namespace ChatApi.Services
{
    public class CosmosDbService : ICosmosDbService
    {
        private Container _sessionContainer;
        private Container _messageContainer;

        public CosmosDbService(
            CosmosClient dbClient,
            string databaseName,
            string sessionContainerName,
            string messageContainerName)
        {
            _sessionContainer = dbClient.GetContainer(databaseName, sessionContainerName);
            _messageContainer = dbClient.GetContainer(databaseName, messageContainerName);
        }

        public async Task<List<Session>> GetSessionsAsync()
        {
            var query = _sessionContainer.GetItemQueryIterator<Session>();
            List<Session> sessions = new List<Session>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                sessions.AddRange(response.ToList());
            }

            return sessions;
        }

        public async Task<List<ChatMessage>> GetSessionMessagesAsync(string sessionId)
        {            
            // Need a query for message container that filters by sessionId
            var query = _messageContainer.GetItemQueryIterator<ChatMessage>(
                new QueryDefinition("SELECT * FROM c WHERE c.sessionId = @sessionId")
                .WithParameter("@sessionId", sessionId));

            List<ChatMessage> messages = new List<ChatMessage>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                messages.AddRange(response.ToList());
            }

            return messages;
        }

        public async Task<Session> GetSessionAsync(string id)
        {         
            var response = await _sessionContainer.ReadItemAsync<Session>(id, new PartitionKey(id));
            return response.Resource;
            
        }

        public async Task<Session> InsertSessionAsync(Session session)
        {
            var response = await _sessionContainer.CreateItemAsync(session, new PartitionKey(session.Id));
            return response.Resource;
        }

        public async Task<ChatMessage> InsertMessageAsync(ChatMessage message)
        {
            var response = await _messageContainer.CreateItemAsync(message, new PartitionKey(message.Id));
            return response.Resource;
        }
    }
}

