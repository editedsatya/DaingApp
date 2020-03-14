

using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.dtos
{ 

public class UserForRegister{

      [Required]
      public string Username { get; set; }
       [Required]
       [StringLength(8,MinimumLength = 4,ErrorMessage= "Passwod should be between 4 to 8 characters!")]
      public string Password { get; set; }
}
}