using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FindYOU;

public class UsersChatsDto
{
 public int Id { get; set; }

    
    public string Title { get; set; }


    public string ChatLink { get; set; }

  
    public string? Summary { get; set; }

    public string? Notes { get; set; }

    public bool IsPublic { get; set; } = false;



    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int CategoryId { get; set; }

public bool IsLiked { get; set; }

public int LikeCount { get; set; }
  
    public Category? Category { get; set; }

     public string? ChatTags { get; set; }

    public int UserId { get; set; }

   
}
