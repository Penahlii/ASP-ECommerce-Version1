using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.WebUI.Models;

public class AddProductViewModel
{
    [Required]
    public string ProductName { get; set; } = null!;

    [Required]
    public int CategoryId { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a valid price.")]
    public decimal UnitPrice { get; set; }

    [Required]
    [Range(1, short.MaxValue, ErrorMessage = "Please enter a valid stock quantity.")]
    public short UnitsInStock { get; set; }

    public List<SelectListItem> Categories { get; set; } = new();
}
