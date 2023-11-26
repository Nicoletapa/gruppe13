using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nøsted.Data;
using Nøsted.Models;
using Nøsted.Entities;


namespace Nøsted.Controllers
{
    /// <summary>
    /// Controller for managing checklists and checklist items.
    /// </summary>
    public class SjekklisteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SjekklisteController(ApplicationDbContext context)
        {
            _context = context;
        }
        
       //Private Methods-----------------------------------------------------------------
        /// <summary>
        /// Groups checklist items by category for a specific checklist.
        /// </summary>
        /// <param name="sjekklisteId">The unique identifier of the checklist.</param>
        /// <returns>A list of SjekkpunktGroup containing grouped checklist items.</returns>
        private async Task<List<SjekkpunktGroup>> GroupSjekkpunkterByKategori(Guid sjekklisteId)
        {
            var sjekklisteSjekkpunkter = await _context.SjekklisteSjekkpunkt
                .Include(ss => ss.sjekkpunkt)
                .ThenInclude(s => s.Kategori)
                .Where(ss => ss.SjekklisteID == sjekklisteId)
                .ToListAsync();

            return sjekklisteSjekkpunkter
                .GroupBy(ss => ss.sjekkpunkt.Kategori.KategoriNavn)
                .Select(group => new SjekkpunktGroup
                {
                    KategoriNavn = group.Key,
                    Sjekkpunkter = group.Select(ss => new SjekkpunktWithStatus
                    {
                        Sjekkpunkt = ss.sjekkpunkt,
                        Status = ss.Status ?? null
                    }).ToList()
                }).ToList();
        }
       
        /// <summary>
        /// Builds a view model for creating a checklist based on checklist ID and grouped checklist items.
        /// </summary>
        /// <param name="sjekklisteId">The unique identifier of the checklist.</param>
        /// <param name="groupedSjekkpunkter">List of grouped checklist items.</param>
        /// <returns>A CreateSjekklisteSjekkpunktViewModel instance.</returns>
        private CreateSjekklisteSjekkpunktViewModel BuildViewModel(Guid sjekklisteId, List<SjekkpunktGroup> groupedSjekkpunkter)
        {
            return new CreateSjekklisteSjekkpunktViewModel()
            {
                SjekklisteId = sjekklisteId,
                GroupedSjekkpunkter = groupedSjekkpunkter
            };
        }

        //Public Methods-------------------------------------------------------------------------
        /// <summary>
        /// Displays details of a checklist based on the provided ID.
        /// </summary>
        /// <param name="id">The unique identifier of the checklist to display.</param>
        /// <returns>
        /// If the checklist exists, displays the details; otherwise, returns a not found error.
        /// </returns>
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupedSjekkpunkter = await GroupSjekkpunkterByKategori(id);

            if (groupedSjekkpunkter == null || !groupedSjekkpunkter.Any())
            {
                return NotFound();
            }

            var viewModel = BuildViewModel(id, groupedSjekkpunkter);
            return View(viewModel);
        }

        /// <summary>
        /// Creates a new checklist for a specific order.
        /// </summary>
        /// <param name="orderId">The ID of the order to create a checklist for.</param>
        /// <returns>
        /// Redirects to the checklist details if a checklist already exists for the order;
        /// otherwise, creates a new checklist and displays it.
        /// </returns>
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
            var kategorier = await _context.Kategori.ToListAsync(); 

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
           
            var groupedSjekkpunkter = await GroupSjekkpunkterByKategori(newSjekklisteId);
            var viewModel = BuildViewModel(newSjekklisteId, groupedSjekkpunkter);

            return View(viewModel);
        }

        /// <summary>
        /// Creates or updates checklist items based on the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model containing checklist data.</param>
        /// <returns>
        /// If the checklist creation or update is successful, redirects to the checklist details;
        /// otherwise, displays an error.
        /// </returns>
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
                            _context.Update(existingEntry); 
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
                
        
        /// <summary>
        /// Edits the checklist items for a specific checklist.
        /// </summary>
        /// <param name="sjekklisteId">The ID of the checklist to edit.</param>
        /// <returns>
        /// If the checklist exists, displays the edit view; otherwise, returns a not found error.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid sjekklisteId)
        {
            if (sjekklisteId == null)
            {
                return NotFound();
            }

            var groupedSjekkpunkter = await GroupSjekkpunkterByKategori(sjekklisteId);

            if (groupedSjekkpunkter == null || !groupedSjekkpunkter.Any())
            {
                return NotFound();
            }

            var viewModel = BuildViewModel(sjekklisteId, groupedSjekkpunkter);
            return View(viewModel);
        }


        /// <summary>
        /// Updates checklist items based on the provided view model.
        /// </summary>
        /// <param name="viewModel">The view model containing updated checklist data.</param>
        /// <returns>
        /// If the checklist update is successful, redirects to the checklist details;
        /// otherwise, displays an error.
        /// </returns>
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
        
        /// <summary>
        /// Displays a checklist for deletion based on the provided ID.
        /// </summary>
        /// <param name="id">The unique identifier of the checklist to delete.</param>
        /// <returns>
        /// If the checklist exists, displays it for deletion; otherwise, returns a not found error.
        /// </returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
                
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
           


       

       
    }
}