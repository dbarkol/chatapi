
namespace ChatApi.Models
{
    public class ChatMessage
    {
        public enum MessageRoles
        {
            User = 0,
            Bot = 1
        }
        
        public string Id { get; set; }

        public string SessionId { get; set; }

        public DateTimeOffset TimeStamp { get; set; }

        public string Content { get; set; }
        
        public string UserId { get; set; }

        public string UserName { get; set; }

        public MessageRoles MessageRole { get; set; }

        
        public ChatMessage(
            string sessionId,
            string userId,
            string userName,
            string content,
            MessageRoles messageRole = MessageRoles.User)        
        {
            Id = Guid.NewGuid().ToString();
            SessionId = sessionId;
            UserId = userId;
            UserName = userName;
            Content = content;
            MessageRole = messageRole;
            TimeStamp = DateTimeOffset.Now;
        }

    }
}