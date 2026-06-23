using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FindYOU;

public class ChatEntry
{
    public int Id { get; set; }

    [Required]
    [StringLength(255)]
    public string Title { get; set; }

    [Required]
    [Url]
    public string ChatLink { get; set; }

    // AI Generated Summary
    public string? Summary { get; set; }

    public string? Notes { get; set; }

    public bool IsPublic { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public Category? Category { get; set; }

     public string? ChatTags { get; set; }

    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

[JsonIgnore]
    public ICollection<Bookmark> Bookmarks { get; set; }
    = new List<Bookmark>();
}