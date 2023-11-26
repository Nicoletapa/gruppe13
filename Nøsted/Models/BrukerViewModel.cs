using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Nøsted.Models;

public class CreateRoleViewModel
{
   [Required]
   public string RoleName { get; set; }
}


public class EditRoleViewModel
{
   public string Id { get; set; }

   [Required(ErrorMessage = "Role Name is required")]
   public string RoleName { get; set; }


}

public class UserRoleViewModel
{
   public string UserId { get; set; }
   public string UserName { get; set; }
   public string Email { get; set; }
   public string CurrentRole { get; set; } // Assuming one role per user for simplicity
   public List<string> AvailableRoles { get; set; } // All available roles
   public string NewRole { get; set; }
}


