using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities
{
  public class User
  {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Password { get; set; } = string.Empty;
        public required string Email { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public bool Active { get; set; } = true;
    }
}
