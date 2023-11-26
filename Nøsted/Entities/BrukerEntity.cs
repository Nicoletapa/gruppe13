using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;


namespace Nøsted.Entities;

public class ApplicationUser : IdentityUser
{
    [Key]
    public override string Id { get; set; }
  
}
