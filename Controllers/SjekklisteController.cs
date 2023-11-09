using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Nøsted.Data;
using Nøsted.Models;

namespace Nøsted.Controllers
{
    public class SjekklisteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SjekklisteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sjekkliste
        public async Task<IActionResult> Index()
        {
            var viewModel = new CreateSjekklisteSjekkpunktViewModel
            {

                sjekklisteSjekkpunkt = await _context.SjekklisteSjekkpunkt.FirstOrDefaultAsync()
            };
            return View(viewModel);
            

        }

        // GET: Sjekkliste/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Sjekkliste == null)
            {
                return NotFound();
            }

            var sjekkliste = await _context.Sjekkliste
                .FirstOrDefaultAsync(m => m.SjekklisteID == id);
            if (sjekkliste == null)
            {
                return NotFound();
            }

            return View(sjekkliste);
        } 
        
        [HttpGet]
        public async Task<IActionResult> Create(int orderId)
        {
            // Assuming you have some logic to retrieve an existing OrdreViewModel based on your requirements
            var existingSjekkliste = await _context.Sjekkliste
                .FirstOrDefaultAsync(s => s.OrdreNr == orderId);

            if (existingSjekkliste == null)
            {
                // If no checklist exists for the given orderID, create a new one
                var newSjekkliste = new Sjekkliste
                {
                    OrdreNr = orderId,
                    
                    // other properties...
                };

                _context.Sjekkliste.Add(newSjekkliste);
                await _context.SaveChangesAsync();

                existingSjekkliste = newSjekkliste;
            }

            var sjekkpunkter = await _context.Sjekkpunkt2.ToListAsync();

            var viewModel = new CreateSjekklisteSjekkpunktViewModel
            {
                sjekklisteSjekkpunkt = new SjekklisteSjekkpunkt
                {
                    sjekkliste = existingSjekkliste,
                    sjekkpunkt = new Sjekkpunkt(),
                    Status = "ok"
                },
                Sjekkpunkter = sjekkpunkter,
                SjekklisteId = existingSjekkliste.SjekklisteID // Set the SjekklisteId in the view model
            };

            return View(viewModel);
        }


        // POST: api/Sjekkliste/CreateChecklist
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSjekklisteSjekkpunktViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Assuming SjekklisteSjekkpunkt has a Status property that you want to set
                viewModel.sjekklisteSjekkpunkt.Status = "Ok"; // Set the default status here or get it from the form

                // Add the new SjekklisteSjekkpunkt to the context and save changes
                _context.SjekklisteSjekkpunkt.Add(viewModel.sjekklisteSjekkpunkt);
                await _context.SaveChangesAsync();

                // Redirect to the desired action after successfully creating the SjekklisteSjekkpunkt
                return RedirectToAction("Index");
            }

            // If the ModelState is not valid, return to the Create view with the same viewModel
            return View(viewModel);
        }
        
        /*
        [HttpGet] 
        public async Task<IActionResult> Create()
        {

        var viewModel = new CreateSjekklisteSjekkpunktViewModel
        {
            sjekklisteSjekkpunkt = new SjekklisteSjekkpunkt()
            {
                sjekkliste = new Sjekkliste(),
                sjekkpunkt = new Sjekkpunkt(),
                    
                    
            },
            
        };
        _context.SjekklisteSjekkpunkt.ToListAsync();
        _context.SaveChangesAsync();
            
        return View(viewModel);
    }
        
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateSjekklisteSjekkpunktViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            // Assuming SjekklisteSjekkpunkt has a Status property that you want to set
            viewModel.sjekklisteSjekkpunkt.Status = viewModel.sjekklisteSjekkpunkt.Status; // Set the default status here or get it from the form

            // Add the new SjekklisteSjekkpunkt to the context and save changes
            _context.SjekklisteSjekkpunkt.Add(viewModel.sjekklisteSjekkpunkt);
            await _context.SaveChangesAsync();

            // Redirect to the desired action after successfully creating the SjekklisteSjekkpunkt
            return RedirectToAction("Index");
        }

        // If the ModelState is not valid, return to the Create view with the same viewModel
        return View(viewModel);
    }
    */



        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int SjekklisteID, int SjekkpunktID, string status)
        {
            // Find the corresponding checklist and checkpoint
            var sjekkliste = await _context.Sjekkliste.FindAsync(SjekklisteID);
            var sjekkpunkt = await _context.Sjekkpunkt2.FindAsync(SjekkpunktID);

            if (sjekkliste == null || sjekkpunkt == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid Sjekkliste or Sjekkpunkt selection.");
                return NotFound(); // Handle not found cases
            }

                // Create a new SjekklisteSjekkpunkt with the provided status
                var sjekklisteSjekkpunkt = new SjekklisteSjekkpunkt
                {
                    Sjekkliste = sjekkliste,
                    sjekkpunkter = sjekkpunkt,
                    Status = status
                };

                _context.SjekklisteSjekkpunkt.Add(sjekklisteSjekkpunkt);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index"); // Redirect to the appropriate view
            }
            */

     

        // POST: Sjekkliste/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SjekklisteID")] Sjekkliste sjekkliste)
        {
            if (id != sjekkliste.SjekklisteID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sjekkliste);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SjekklisteExists(sjekkliste.SjekklisteID))
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
            return View(sjekkliste);
        }

        // GET: Sjekkliste/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Sjekkliste == null)
            {
                return NotFound();
            }

            var sjekkliste = await _context.Sjekkliste
                .FirstOrDefaultAsync(m => m.SjekklisteID == id);
            if (sjekkliste == null)
            {
                return NotFound();
            }

            return View(sjekkliste);
        }

        // POST: Sjekkliste/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sjekkliste == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Sjekkliste'  is null.");
            }
            var sjekkliste = await _context.Sjekkliste.FindAsync(id);
            if (sjekkliste != null)
            {
                _context.Sjekkliste.Remove(sjekkliste);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SjekklisteExists(int id)
        {
          return (_context.Sjekkliste?.Any(e => e.SjekklisteID == id)).GetValueOrDefault();
        }
    }
}
