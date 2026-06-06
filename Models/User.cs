using System.ComponentModel.DataAnnotations;

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
}