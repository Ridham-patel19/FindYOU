using System.Text.Json.Serialization;

namespace FindYOU;

public class Like
{
    public int? Id { get; set; }

    public int UserId { get; set; }

    [JsonIgnore]
    public User User { get; set; }

    public int ChatEntryId { get; set; }
     [JsonIgnore]
    public ChatEntry ChatEntry { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
