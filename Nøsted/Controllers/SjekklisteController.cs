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
       
        // GET: Sjekkliste/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sjekklisteSjekkpunkter = await _context.SjekklisteSjekkpunkt
                .Where(sl => sl.SjekklisteID == id)
                .Include(sl => sl.sjekkpunkt)
                .ThenInclude(s => s.Kategori)
                .ToListAsync();

            if (sjekklisteSjekkpunkter == null || !sjekklisteSjekkpunkter.Any())
            {
                return NotFound();
            }

            var viewModel = new CreateSjekklisteSjekkpunktViewModel()
            {
                SjekklisteId = id,
                SjekkpunkterWithStatus = sjekklisteSjekkpunkter.Select(sls => new SjekkpunktWithStatus
                {
                    Sjekkpunkt = sls.sjekkpunkt,
                    Status = sls.Status
                }).ToList()
            };

            return View(viewModel);
        }

        
[HttpGet]
public async Task<IActionResult> Create(int orderId)
{
    var existingChecklist = await _context.SjekklisteSjekkpunkt
        .Where(sl => sl.OrdreNr == orderId)
        .FirstOrDefaultAsync();

    if (existingChecklist != null)
    {
        return RedirectToAction("Details", new { id = existingChecklist.SjekklisteID });
    }

    var newSjekklisteId = Guid.NewGuid();
    var sjekkpunkter = await _context.Sjekkpunkt
        .Include(sp => sp.Kategori)
        .ToListAsync();
    var sjekklisteSjekkpunktList = new List<SjekklisteSjekkpunkt>();
    
    foreach (var sjekkpunkt in sjekkpunkter)
    {
        sjekklisteSjekkpunktList.Add(new SjekklisteSjekkpunkt
        {
            SjekklisteID = newSjekklisteId,
            SjekkpunktID = sjekkpunkt.SjekkpunktID,
            OrdreNr = orderId,
            Status = "OK" ,// Default status
            sjekkpunkt = sjekkpunkt
        });
    }

    _context.SjekklisteSjekkpunkt.AddRange(sjekklisteSjekkpunktList);
    await _context.SaveChangesAsync();

    var viewModel = new CreateSjekklisteSjekkpunktViewModel
    {
        SjekklisteId = newSjekklisteId,
        SjekkpunkterWithStatus = sjekkpunkter.Select(sp => new SjekkpunktWithStatus
        {
            Sjekkpunkt = sp,
            Status = "OK" // default status
        }).ToList()
    };

    return View(viewModel);
}

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(CreateSjekklisteSjekkpunktViewModel viewModel)
{
    if (ModelState.IsValid)
    {
        foreach (var sjekkpunktStatus in viewModel.SjekkpunkterWithStatus)
        {
            var existingEntry = _context.SjekklisteSjekkpunkt.FirstOrDefault(
                s => s.SjekkpunktID == sjekkpunktStatus.Sjekkpunkt.SjekkpunktID
                     && s.SjekklisteID == viewModel.SjekklisteId);

            if (existingEntry != null)
            {
                existingEntry.Status = sjekkpunktStatus.Status;
                _context.Entry(existingEntry).State = EntityState.Modified; // Mark entity as modified
            }
            else
            {
                var sjekklisteSjekkpunkt = new SjekklisteSjekkpunkt
                {
                    SjekkpunktID = sjekkpunktStatus.Sjekkpunkt.SjekkpunktID,
                    Status = sjekkpunktStatus.Status,
                    SjekklisteID = viewModel.SjekklisteId,
                    OrdreNr = viewModel.OrdreNr
                };
                _context.SjekklisteSjekkpunkt.Add(sjekklisteSjekkpunkt);
            }
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Details", new { id = viewModel.SjekklisteId });
    }

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
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("SjekklisteSjekkpunktID")] SjekklisteSjekkpunkt sjekkliste)
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
        }*/

        // GET: Sjekkliste/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var sjekkliste = await _context.SjekklisteSjekkpunkt
                .FirstOrDefaultAsync(sl => sl.SjekklisteID == id);

            if (sjekkliste == null)
            {
                return NotFound(); // Sjekkliste not found
            }

            int orderId = sjekkliste.OrdreNr; // Assuming OrdreNr is a property of SjekklisteSjekkpunkt

            // Find all SjekklisteSjekkpunkt entries related to the orderId
            var sjekklisteSjekkpunkter = await _context.SjekklisteSjekkpunkt
                .Where(sl => sl.OrdreNr == orderId)
                .ToListAsync();

            if (sjekklisteSjekkpunkter == null || !sjekklisteSjekkpunkter.Any())
            {
                return NotFound(); // No SjekklisteSjekkpunkt found for the given orderId
            }

            // Remove the found entries from the database context
            _context.SjekklisteSjekkpunkt.RemoveRange(sjekklisteSjekkpunkter);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Redirect to a success page, or another appropriate action
            return RedirectToAction("Index", "Ordre"); // Replace 'Index' with your desired landing page after deletion
        }

       

        // POST: Sjekkliste/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed (Guid id)
        {
            var sjekklisteSjekkpunkter = await _context.SjekklisteSjekkpunkt
                .Where(sl => sl.SjekklisteID == id)
                .ToListAsync();

            if (sjekklisteSjekkpunkter == null || !sjekklisteSjekkpunkter.Any())
            {
                return NotFound(); // Handle the case where no checklist is found
            }

            _context.SjekklisteSjekkpunkt.RemoveRange(sjekklisteSjekkpunkter);
            await _context.SaveChangesAsync();

            // Redirect to a success page or another appropriate action
            return RedirectToAction("Details", "Ordre"); // Replace 'Index' with your desired landing page
        }
        // private bool SjekklisteExists(int id)
        // {
        //     return (_context.SjekklisteSjekkpunkt?.Any(e => e.SjekklisteID == id)).GetValueOrDefault();
        // }
    }
}