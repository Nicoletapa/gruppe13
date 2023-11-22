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
                    CurrentRole = currentRole // Setting the CurrentRole for the user
                });
            }

            return View(model);
        }


        
        
        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

    

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View();

            }
            var model = new EditRoleViewModel()
            {
                Id = role.Id,
                RoleName = role.Name,
                
            };
            
            return View(model);

        }
        
        
        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                return View();

            }
            else
            {
                role.Name = model.RoleName;

                // Update the Role using UpdateAsync
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }

        
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
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

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("ListRoles");
            }
            else
            {
                var result = await _roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("ListRoles");
            }
        }

        
        
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // Handle user not found
                return NotFound();
            }
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



        [HttpPost]
        public async Task<IActionResult> EditUserRoles(UserRoleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                // Handle user not found
                return NotFound();
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            

            if (!currentRoles.Contains(model.CurrentRole))
            {
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, model.NewRole);
            }

            // Redirect after successful update
            return RedirectToAction("ListUsers");
        }


        private bool BrukerExists(string id)
        {
          return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        
        
        // POST: Bruker/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                // User not found, handle accordingly
                return NotFound();
            }

            // Get the roles associated with the user
            var roles = await _userManager.GetRolesAsync(user);

            // Remove user from roles
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
          

            // Delete the user
            result = await _userManager.DeleteAsync(user);
            

            // Redirect to the user list or a confirmation page after deletion
            return RedirectToAction(nameof(ListUsers));
        }

         /*// GET: Bruker/Create
                public IActionResult Create()
                {
                    return View();
                }
        
                // POST: Bruker/Create
                // To protect from overposting attacks, enable the specific properties you want to bind to.
                // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Create([Bind("Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] Bruker bruker)
                {
                    if (ModelState.IsValid)
                    {
                        _context.Add(bruker);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    return View(bruker);
                }*/

        
    }
}