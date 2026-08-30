using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BankSystem.Models.AI;

public class KnowledgeUploadViewModel
{
    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Category { get; set; } = string.Empty;

    [Required]
    public IFormFile? File { get; set; }
}