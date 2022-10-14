using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.Models.Book;

public class BookUpdateDto : BaseDto
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; }

    [Required]
    public int Year { get; set; }

    [Required] public string Isbn { get; set; }

    [Required]
    [StringLength(500, MinimumLength = 10)]
    public string Summary { get; set; }

    public string Image { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public decimal Price { get; set; }
}