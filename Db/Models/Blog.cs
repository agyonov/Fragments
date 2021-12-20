using System.Text.Json.Serialization;

namespace Db
{
    public class Blog
    {
        public int Id { get; set; }
        public string? Url { get; set; }

        [JsonIgnore]
        public virtual List<Post> Posts { get; } = default!;
    }
}
