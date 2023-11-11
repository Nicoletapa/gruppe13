using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Nøsted.Models;

public class Bruker : IdentityUser
{
   [Key]
   public override string Id { get; set; }
  
}



public class CreateRoleViewModel
{
   [Required]
   public string RoleName { get; set; }
}

public class EditRoleViewModel
{
   public EditRoleViewModel()
   {
      Users = new List<string>();
   }
   public string Id { get; set; }
   
   [Required(ErrorMessage = "Role Name is required")]
   public string RoleName { get; set; }

   public List<string> Users { get; set; } = new();
}