using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nøsted.Data;
using Nøsted.Models;

namespace Nøsted.Controllers
{
    public class BrukerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public BrukerController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser>userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;

        }
        //private metoder
        /// <summary>
        /// Retrieves a user by their ID or returns an error view if the user is not found.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve.</param>
        /// <returns>An IActionResult representing either the user or an error view.</returns>
        private async Task<IActionResult> GetUserOrNotFoundResult(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with ID = {userId} not found.";
                return View("Error"); // A view that shows the error message
            }
            return null; // User found, return null to indicate no error
        }

        /// <summary>
        /// Retrieves a role by its ID or returns an error view if the role is not found.
        /// </summary>
        /// <param name="roleId">The ID of the role to retrieve.</param>
        /// <returns>An IActionResult representing either the role or an error view.</returns>
        private async Task<IActionResult> GetRoleOrNotFoundResult(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with ID = {roleId} not found.";
                return View("Error"); // A view that shows the error message
            }
            return null; // Role found, return null to indicate no error
        }
        /// <summary>
        /// Adds errors from IdentityResult to the ModelState.
        /// </summary>
        /// <param name="result">The IdentityResult containing errors.</param>
        private void AddErrorsToModelState(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        //Actions
        /// <summary>
        /// Displays a list of users with an optional search term filter.
        /// </summary>
        /// <param name="searchTerm">The search term to filter users.</param>
        /// <returns>A view with the list of users.</returns>
        [HttpGet]
        public async Task<IActionResult> ListUsers(string searchTerm)
        {
            var users = await _userManager.Users.ToListAsync();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                users = users.Where(user => user.UserName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) 
                                            || user.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                                            || _userManager.GetRolesAsync(user).Result.Contains(searchTerm, StringComparer.OrdinalIgnoreCase))
                    .ToList();
            }

            var model = new List<UserRoleViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var currentRole = roles.FirstOrDefault(); // Gets the first role, or null if there are no roles

                model.Add(new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    CurrentRole = currentRole 
                });
            }

            return View(model);
        }


        /// <summary>
        /// Displays a list of roles.
        /// </summary>
        /// <returns>A view with the list of roles.</returns>
        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        /// <summary>
        /// Displays the edit role page for a specified role ID.
        /// </summary>
        /// <param name="id">The ID of the role to edit.</param>
        /// <returns>A view with the role to edit.</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var notFoundResult = await GetRoleOrNotFoundResult(id);
            if (notFoundResult != null)
            {
                return notFoundResult; 
            }
            
            var role = await _roleManager.FindByIdAsync(id);
            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var notFoundResult = await GetRoleOrNotFoundResult(model.Id);
            if (notFoundResult != null)
            {
                return notFoundResult; 
            }
            
            var role = await _roleManager.FindByIdAsync(model.Id);
            role.Name = model.RoleName;

            // Update the Role using UpdateAsync
            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("ListRoles");
            }
            AddErrorsToModelState(result);
            return View(model);
        }
        // Create a new role
        /// <summary>
        /// Displays the create role page.
        /// </summary>
        /// <returns>A view to create a new role.</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        /// <summary>
        /// Handles the POST request to create a new role.
        /// </summary>
        /// <param name="model">The model containing role information.</param>
        /// <returns>Redirects to the list of roles or shows errors on failure.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole()
                {
                    Name = model.RoleName
                };
                IdentityResult result = await _roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                AddErrorsToModelState(result);

            }

            return View(model);
        }
        
        // Delete a role
        /// <summary>
        /// Handles the POST request to delete a role by its ID.
        /// </summary>
        /// <param name="id">The ID of the role to delete.</param>
        /// <returns>Redirects to the list of roles or shows errors on failure.</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var notFoundResult = await GetRoleOrNotFoundResult(id);
            if (notFoundResult != null)
            {
                return notFoundResult; // Shows the not found message if the user is not found
            }

            
            var role = await _roleManager.FindByIdAsync(id);
            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("ListRoles");
            }

            AddErrorsToModelState(result);
            return View("ListRoles"); 
        }


        // Edit user roles
        /// <summary>
        /// Displays the edit user roles page for a specified user ID.
        /// </summary>
        /// <param name="userId">The ID of the user to edit roles for.</param>
        /// <returns>A view to edit user roles.</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditUserRoles(string userId)
        {
            var notFoundResult = await GetUserOrNotFoundResult(userId);
            if (notFoundResult != null)
            {
                return notFoundResult; // Shows the not found message if the user is not found
            }
            

            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.Roles = _roleManager.Roles
                .Where(r => r.Name != "Admin") // Filter out the "Admin" role
                .Select(r => r.Name)
                .ToList();

            var userRoleViewModel = new UserRoleViewModel
            {
                UserId = userId,
                UserName = user.UserName,
                Email = user.Email,
                CurrentRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault(),
                AvailableRoles = _roleManager.Roles.Select(r => r.Name).ToList()
            };

            return View(userRoleViewModel);
        }



        /// <summary>
        /// Handles the POST request to edit user roles.
        /// </summary>
        /// <param name="model">The model containing user role information.</param>
        /// <returns>Redirects to the list of users after updating roles.</returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> EditUserRoles(UserRoleViewModel model)
        {
            var notFoundResult = await GetUserOrNotFoundResult(model.UserId);
            if (notFoundResult != null)
            {
                return notFoundResult; // Shows the not found message if the user is not found
            }

            var user = await _userManager.FindByIdAsync(model.UserId);
            var currentRoles = await _userManager.GetRolesAsync(user);
    
            if (!currentRoles.Contains(model.CurrentRole))
            {
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, model.NewRole);
            }

            return RedirectToAction("ListUsers");
        }


        /// <summary>
        /// Checks if a user exists by ID.
        /// </summary>
        /// <param name="id">The ID of the user to check.</param>
        /// <returns>True if the user exists, otherwise false.</returns>
       
        private bool BrukerExists(string id)
        {
          return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        
        // Delete a user
        /// <summary>
        /// Handles the POST request to delete a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>Redirects to the list of users after deletion.</returns>
        // POST: Bruker/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with ID = {id} not found.";
                return View("Error"); // A view that shows the error message
            }

            var roles = await _userManager.GetRolesAsync(user);

            // Remove user from roles and delete the user
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            result = await _userManager.DeleteAsync(user);

            return RedirectToAction(nameof(ListUsers));
        }
    }
}