using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ToDoPlatform.Models;

public class AppUser : IdentityUser
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [StringLength(300)]
    public string ProfilePicture { get; set; }
}