using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nøsted.Data;
using Nøsted.Models;

namespace Nøsted.Controllers
{
    public class OrdreController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdreController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Ordre
        public async Task<IActionResult> Index()
        {
              return _context.Ordre1 != null ? 
                          View(await _context.Ordre1.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Ordre1'  is null.");
        }

        // GET: Ordre/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Ordre1 == null)
            {
                return NotFound();
            }

            var ordreViewModel = await _context.Ordre1
                .FirstOrDefaultAsync(m => m.OrdreNr == id);
            if (ordreViewModel == null)
            {
                return NotFound();
            }

            return View(ordreViewModel);
        }

        // GET: Ordre/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ordre/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrdreNr,Navn,TelefonNr,Adresse,Type,Gjelder,Epost,Uke,Registrert,Bestilling,AvtaltLevering,ProduktMotatt,AvtaltFerdigstillelse,ServiceFerdig,AntallTimer,Status")] OrdreViewModel ordreViewModel)
        {
            ModelState.Remove("OrdreNr");
            if (ModelState.IsValid)
               {
                    _context.Add(ordreViewModel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
               }
            return View(ordreViewModel);
        }
        // GET: Ordre/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Ordre1 == null)
            {
                return NotFound();
            }

            var ordreViewModel = await _context.Ordre1.FindAsync(id);
            if (ordreViewModel == null)
            {
                return NotFound();
            }
            return View(ordreViewModel);
        }

        // POST: Ordre/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrdreNr,Navn,TelefonNr,Adresse,Type,Gjelder,Epost,Uke,Registrert,Bestilling,AvtaltLevering,ProduktMotatt,AvtaltFerdigstillelse,ServiceFerdig,AntallTimer,status")] OrdreViewModel ordreViewModel)
        {
            if (id != ordreViewModel.OrdreNr)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ordreViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdreViewModelExists(ordreViewModel.OrdreNr))
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
            return View(ordreViewModel);
        }

        // GET: Ordre/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Ordre1 == null)
            {
                return NotFound();
            }

            var ordreViewModel = await _context.Ordre1
                .FirstOrDefaultAsync(m => m.OrdreNr == id);
            if (ordreViewModel == null)
            {
                return NotFound();
            }

            return View(ordreViewModel);
        }

        // POST: Ordre/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Ordre1 == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Ordre1'  is null.");
            }
            var ordreViewModel = await _context.Ordre1.FindAsync(id);
            if (ordreViewModel != null)
            {
                _context.Ordre1.Remove(ordreViewModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrdreViewModelExists(int id)
        {
          return (_context.Ordre1?.Any(e => e.OrdreNr == id)).GetValueOrDefault();
        }
    }
}
