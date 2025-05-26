using System.ComponentModel.DataAnnotations;

namespace GbgMerch.WebUI.Models;

public class ContactFormModel
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(1000, MinimumLength = 10)]
    public string Message { get; set; } = string.Empty;
}
