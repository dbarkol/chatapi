
namespace ChatApi.Models
{
    public class Session
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public Session(string title)
        {
            Id = Guid.NewGuid().ToString();
            Title = title;
            CreatedOn = DateTimeOffset.Now;
        }
    }
}