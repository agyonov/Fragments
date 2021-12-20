using System.Text.Json.Serialization;

namespace Db
{
    public class Post
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }

        public int BlogId { get; set; }
        [JsonIgnore]
        public virtual Blog Blog { get; set; } = default!;
    }
}
