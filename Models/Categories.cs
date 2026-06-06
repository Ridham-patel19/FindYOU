using System.ComponentModel.DataAnnotations;

namespace FindYOU;

  public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public ICollection<ChatEntry>? ChatEntries { get; set; }
    }
