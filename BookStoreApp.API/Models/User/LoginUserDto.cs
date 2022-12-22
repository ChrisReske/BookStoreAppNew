using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.Models.User;

public class LoginUserDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

}