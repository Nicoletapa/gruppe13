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

        
       
        // GET: Sjekkliste/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sjekkliste = await _context.SjekklisteSjekkpunkt
                .Where(sl => sl.SjekklisteID == id)
                .Include(sl => sl.sjekkpunkt)
                .ThenInclude(s => s.Kategori)
                .ToListAsync();

            if (sjekkliste == null || !sjekkliste.Any())
            {
                return NotFound();
            }
            var groupedSjekkpunkter = sjekkliste
                .GroupBy(sl => sl.sjekkpunkt.Kategori.KategoriNavn)
                .Select(group => new SjekkpunktGroup
                {
                    KategoriNavn = group.Key,
                    Sjekkpunkter = group.Select(sls => new SjekkpunktWithStatus
                    {
                        Sjekkpunkt = sls.sjekkpunkt,
                        Status = sls.Status != null ? sls.Status : "Null" 
                    }).ToList()
                }).ToList();

            var viewModel = new CreateSjekklisteSjekkpunktViewModel()
            {
                SjekklisteId = id,
                GroupedSjekkpunkter = groupedSjekkpunkter
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
    var sjekkpunkter = await _context.Sjekkpunkt.ToListAsync();
    var kategorier = await _context.Kategori.ToListAsync(); // Fetch categories

    var sjekklisteSjekkpunktList = new List<SjekklisteSjekkpunkt>();
    
    foreach (var sjekkpunkt in sjekkpunkter)
    {
        sjekklisteSjekkpunktList.Add(new SjekklisteSjekkpunkt
        {
            SjekklisteID = newSjekklisteId,
            SjekkpunktID = sjekkpunkt.SjekkpunktID,
            OrdreNr = orderId,
            Status = null ,// Default status
            sjekkpunkt = sjekkpunkt
        });
    }
    

    _context.SjekklisteSjekkpunkt.AddRange(sjekklisteSjekkpunktList);
    await _context.SaveChangesAsync();
   
    var groupedSjekkpunkter = sjekkpunkter
            
            .GroupBy(sp => sp.Kategori != null ? sp.Kategori.KategoriNavn : "Uncategorized")
            .Select(group => new SjekkpunktGroup
            {
                KategoriNavn = group.Key,
                Sjekkpunkter = group.Select(sp => new SjekkpunktWithStatus
                {
                    Sjekkpunkt = sp,
                    Status = null
                }).ToList()
            }).ToList();

    var viewModel = new CreateSjekklisteSjekkpunktViewModel
    {
        GroupedSjekkpunkter = groupedSjekkpunkter , // Ensure it's not null
        SjekklisteId = newSjekklisteId,
        Kategorier = kategorier

        
        // Other properties as needed
    };

    return View(viewModel);
}

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(CreateSjekklisteSjekkpunktViewModel viewModel)
{
    if (ModelState.IsValid)
    {
        foreach (var group in viewModel.GroupedSjekkpunkter)
        {
            foreach (var sjekkpunktStatus in group.Sjekkpunkter)
            {
                var existingEntry = await _context.SjekklisteSjekkpunkt.FirstOrDefaultAsync(
                    s => s.SjekkpunktID == sjekkpunktStatus.Sjekkpunkt.SjekkpunktID
                         && s.SjekklisteID == viewModel.SjekklisteId);

                if (existingEntry != null)
                {
                    existingEntry.Status = sjekkpunktStatus.Status;
                    _context.Update(existingEntry); // Make sure to update the entity in the context
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
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Details", new { id = viewModel.SjekklisteId });
    }

    return View(viewModel);
}
        

[HttpGet]
public async Task<IActionResult> Edit(Guid sjekklisteId)
{
    var sjekklisteSjekkpunkter = await _context.SjekklisteSjekkpunkt
        .Include(ss => ss.sjekkpunkt)
        .ThenInclude(s => s.Kategori)
        .Where(ss => ss.SjekklisteID == sjekklisteId)
        .ToListAsync();

    if (sjekklisteSjekkpunkter == null || !sjekklisteSjekkpunkter.Any())
    {
        return NotFound();
    }

    var groupedSjekkpunkter = sjekklisteSjekkpunkter
        .GroupBy(ss => ss.sjekkpunkt.Kategori.KategoriNavn)
        .Select(group => new SjekkpunktGroup
        {
            KategoriNavn = group.Key,
            Sjekkpunkter = group.Select(ss => new SjekkpunktWithStatus
            {
                Sjekkpunkt = ss.sjekkpunkt,
                Status = ss.Status ?? "Null"
            }).ToList()
        }).ToList();

    var viewModel = new CreateSjekklisteSjekkpunktViewModel()
    {
        SjekklisteId = sjekklisteId,
        GroupedSjekkpunkter = groupedSjekkpunkter
        // Populate other necessary fields
    };

    return View(viewModel);
}



[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(CreateSjekklisteSjekkpunktViewModel viewModel)
{
    if (ModelState.IsValid)
    {
        // Fetch existing entries
        var existingEntries = await _context.SjekklisteSjekkpunkt
            .Where(ss => ss.SjekklisteID == viewModel.SjekklisteId)
            .ToListAsync();

        // Update each entry
        foreach (var group in viewModel.GroupedSjekkpunkter)
        {
            foreach (var sjekkpunktStatus in group.Sjekkpunkter)
            {
                var existingEntry = existingEntries.FirstOrDefault(
                    e => e.SjekkpunktID == sjekkpunktStatus.Sjekkpunkt.SjekkpunktID);

                if (existingEntry != null)
                {
                    existingEntry.Status = sjekkpunktStatus.Status;
                    _context.Update(existingEntry); // Update the entity in the context
                }
            }
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Details", new { id = viewModel.SjekklisteId });
    }

    // If we got this far, something failed, redisplay form
    return View(viewModel);
}
        
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

    return View(sjekklisteSjekkpunkter);
}
        // GET: Sjekkliste/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed (Guid id)        {
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
       


       

        // private bool SjekklisteExists(int id)
        // {
        //     return (_context.SjekklisteSjekkpunkt?.Any(e => e.SjekklisteID == id)).GetValueOrDefault();
        // }
    }
}