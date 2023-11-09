using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public IActionResult ListUsers()
        {
            var users = _userManager.Users;
            return View(users);
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
                Users = new List<string>() // Initialize the Users property
            };
            foreach (var user in _userManager.Users.ToList())
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

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
                    return RedirectToAction("Index", "Home");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        // GET: Bruker
        public async Task<IActionResult> Index()
        {
              return _context.bruker != null ? 
                          View(await _context.bruker.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Bruker'  is null.");
        }

        // GET: Bruker/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.bruker == null)
            {
                return NotFound();
            }

            var bruker = await _context.bruker
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bruker == null)
            {
                return NotFound();
            }

            return View(bruker);
        }

        // GET: Bruker/Create
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
        }

        // GET: Bruker/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.bruker == null)
            {
                return NotFound();
            }

            var bruker = await _context.bruker.FindAsync(id);
            if (bruker == null)
            {
                return NotFound();
            }
            return View(bruker);
        }

        // POST: Bruker/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] Bruker bruker)
        {
            if (id != bruker.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bruker);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrukerExists(bruker.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bruker);
        }

        // GET: Bruker/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.bruker == null)
            {
                return NotFound();
            }

            var bruker = await _context.bruker
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bruker == null)
            {
                return NotFound();
            }

            return View(bruker);
        }

        // POST: Bruker/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.bruker == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Bruker'  is null.");
            }
            var bruker = await _context.bruker.FindAsync(id);
            if (bruker != null)
            {
                _context.bruker.Remove(bruker);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrukerExists(string id)
        {
          return (_context.bruker?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
