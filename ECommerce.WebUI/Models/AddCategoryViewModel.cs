using System.ComponentModel.DataAnnotations;

namespace ECommerce.WebUI.Models;

public class AddCategoryViewModel
{
    [Required]
    [MinLength(3)]
    public string? CategoryName { get; set; }
}
