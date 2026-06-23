using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FindYOU;

public class User
{
    public int Id { get; set; }



[StringLength(100)]
[Required]
    public string Name { get; set; }
[StringLength(100)]
[Required]
[EmailAddress]
    public string Email { get; set; }


[StringLength(100)]
[Required]
    public string password { get ; set;}

    public string Role {get ; set ;} = "User";

     public string? InterestTags { get; set; }

[JsonIgnore]
    public ICollection<ChatEntry> ChatEntries { get; set; }
    = new List<ChatEntry>();

[JsonIgnore]
    public ICollection<Bookmark> Bookmarks { get; set; }
    = new List<Bookmark>();
}