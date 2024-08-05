using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using ChatApi.Interfaces;
using ChatApi.Models;

namespace ChatApi
{
    public class Chat
    {
        private readonly ILogger _logger;
        private readonly ICosmosDbService _cosmosDbService;

        public Chat(ILoggerFactory loggerFactory, ICosmosDbService cosmosDbService)
        {
            _logger = loggerFactory.CreateLogger<Chat>();
            _cosmosDbService = cosmosDbService;
        }

        // Create a function called "Sessions" that returns a list of sessions
        [Function("GetSessions")]
        public async Task<HttpResponseData> Sessions([HttpTrigger(AuthorizationLevel.Function, "get", Route = "sessions")] HttpRequestData req)
        {
            _logger.LogInformation("Get sessions invoked");

            var sessions = await _cosmosDbService.GetSessionsAsync();
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(sessions);

            return response;
        }

        [Function("GetSession")]
        public async Task<HttpResponseData> GetSession([HttpTrigger(AuthorizationLevel.Function, "get", Route = "session/{id}")] HttpRequestData req, string id)
        {
            _logger.LogInformation("Get session invoked");

            var session = await _cosmosDbService.GetSessionAsync(id);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(session);

            return response;
        }

        // Create a function that inserts a session
        [Function("InsertSession")]
        public async Task<HttpResponseData> InsertSession([HttpTrigger(AuthorizationLevel.Function, "post", Route = "session")] HttpRequestData req)
        {
            _logger.LogInformation("Insert session invoked");

            var session = await req.ReadFromJsonAsync<Session>();
            if (session == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            var insertedSession = await _cosmosDbService.InsertSessionAsync(session);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(insertedSession);

            return response;
        }

        [Function("InsertMessage")]
        public async Task<HttpResponseData> InsertMessage([HttpTrigger(AuthorizationLevel.Function, "post", Route = "message")] HttpRequestData req)
        {
            _logger.LogInformation("Insert message invoked");            

            var message = await req.ReadFromJsonAsync<ChatMessage>();
            if (message == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            var insertedMessage = await _cosmosDbService.InsertMessageAsync(message);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(insertedMessage);

            return response;
        }

        [Function("GetSessionMessagesAsync")]
        public async Task<HttpResponseData> GetSessionMessagesAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "session/{sessionId}/messages")] HttpRequestData req, string sessionId)
        {
            _logger.LogInformation("Get session messages invoked");
            
            if (string.IsNullOrEmpty(sessionId))
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            var messages = await _cosmosDbService.GetSessionMessagesAsync(sessionId);            
            if (messages == null)
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(messages);

            return response;
        }
    }
}
