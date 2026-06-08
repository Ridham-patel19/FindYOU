using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FindYOU;

  public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }


[JsonIgnore]
        public ICollection<ChatEntry>? ChatEntries { get; set; }
    }
