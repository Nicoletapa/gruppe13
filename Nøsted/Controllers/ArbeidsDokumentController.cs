using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Nøsted.Data;
using Nøsted.Models;

namespace Nøsted.Controllers
{
    public class ArbeidsDokumentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArbeidsDokumentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ArbeidsDokument
        // public async Task<IActionResult> Index()
        // {
        //       return _context.ArbeidsDokument != null ? 
        //                   View(await _context.ArbeidsDokument.ToListAsync()) :
        //                   Problem("Entity set 'ApplicationDbContext.ArbeidsDokumentViewModel'  is null.");
        // }

      public async Task<IActionResult> Index()
        {
             var arbeidsDokumentViewModels = await _context.ArbeidsDokument2.ToListAsync();
             return View(arbeidsDokumentViewModels);
            

        }
// public IActionResult Index()
            // {
            //     // Get a collection of ArbeidsDokumentViewModel objects
            //     var viewModelList = ArbeidsDokumentViewModels(); // Replace this with your actual data retrieval logic
            //
            //     return View(viewModelList);
            // }
        
        // GET: ArbeidsDokument/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ArbeidsDokument2 == null)
            {
                return NotFound();
            }

            var arbeidsDokumentViewModel = await _context.ArbeidsDokument2
                .FirstOrDefaultAsync(m => m.OrdreID == id);
            if (arbeidsDokumentViewModel == null)
            {
                return NotFound();
            }

            return View(arbeidsDokumentViewModel);
        }

        // GET: ArbeidsDokument/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ArbeidsDokument/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Uke,Registrert,Status,Type,Bestilling,KundeId,AvtaltLevering,ProduktMotatt,AvtaltFerdigstillelse,ServiceFerdig,AntallTimer,OrdreID")] ArbeidsDokumentViewModel arbeidsDokumentViewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(arbeidsDokumentViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(arbeidsDokumentViewModel);
        }

        // GET: ArbeidsDokument/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ArbeidsDokument2 == null)
            {
                return NotFound();
            }

            var arbeidsDokumentViewModel = await _context.ArbeidsDokument2.FindAsync(id);
            if (arbeidsDokumentViewModel == null)
            {
                return NotFound();
            }
            return View(arbeidsDokumentViewModel);
        }

        // POST: ArbeidsDokument/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Uke,Registrert,Status,Type,Bestilling,KundeId,AvtaltLevering,ProduktMotatt,AvtaltFerdigstillelse,ServiceFerdig,AntallTimer,OrdreID")] ArbeidsDokumentViewModel arbeidsDokumentViewModel)
        {
            if (id != arbeidsDokumentViewModel.OrdreID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(arbeidsDokumentViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArbeidsDokumentViewModelExists(arbeidsDokumentViewModel.OrdreID))
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
            return View(arbeidsDokumentViewModel);
        }

        // GET: ArbeidsDokument/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ArbeidsDokument2 == null)
            {
                return NotFound();
            }

            var arbeidsDokumentViewModel = await _context.ArbeidsDokument2
                .FirstOrDefaultAsync(m => m.OrdreID == id);
            if (arbeidsDokumentViewModel == null)
            {
                return NotFound();
            }

            return View(arbeidsDokumentViewModel);
        }

        // POST: ArbeidsDokument/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ArbeidsDokument2 == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ArbeidsDokumentViewModel'  is null.");
            }
            var arbeidsDokumentViewModel = await _context.ArbeidsDokument2.FindAsync(id);
            if (arbeidsDokumentViewModel != null)
            {
                _context.ArbeidsDokument2.Remove(arbeidsDokumentViewModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArbeidsDokumentViewModelExists(int id)
        {
          return (_context.ArbeidsDokument2?.Any(e => e.OrdreID == id)).GetValueOrDefault();
        }
        
    }
}
