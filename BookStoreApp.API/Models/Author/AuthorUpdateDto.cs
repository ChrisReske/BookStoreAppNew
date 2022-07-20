using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.Models.Author;

public class AuthorUpdateDto : BaseDto
{
    [Required]
    [StringLength(50)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(50)]
    public string LastName { get; set; }

    [Required]
    [StringLength(500)]
    public string Bio { get; set; }

}