using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nøsted.Data;
using Nøsted.Models;

namespace Nøsted.Controllers
{
    /// <summary>
    /// Controller for managing orders and their completion status.
    /// </summary>
    [Authorize] // Restrict access to authorized users
    public class OrdreController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdreController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        // Private Methods -----------------------------------------------------------

        /// <summary>
        /// Fetches an order view model or returns a "Not Found" result if the order is not found.
        /// </summary>
        /// <param name="id">The ID of the order to retrieve.</param>
        /// <param name="viewName">The name of the view to render.</param>
        /// <returns>An IActionResult representing either the order or a "Not Found" result.</returns>
        private async Task<IActionResult> FetchOrdreViewModelOrNotFoundAsync(int? id, string viewName)
        {
            if (id == null || _context.Ordre1 == null)
            {
                return NotFound();
            }

            var ordreViewModel = await _context.Ordre1.FirstOrDefaultAsync(m => m.OrdreNr == id);
            if (ordreViewModel == null)
            {
                return NotFound();
            }

            return View(viewName, ordreViewModel);
        }
        
        /// <summary>
        /// Checks if an order view model with the given ID exists.
        /// </summary>
        /// <param name="id">The ID of the order to check.</param>
        /// <returns>True if the order exists; otherwise, false.</returns>
        private bool OrdreViewModelExists(int id)
        {
            return (_context.Ordre1?.Any(e => e.OrdreNr == id)).GetValueOrDefault();
        }
       
        /// <summary>
        /// Calculates the completion percentage of an order based on its checkpoints.
        /// </summary>
        /// <param name="ordreNr">The order number.</param>
        /// <returns>The completion percentage as a float.</returns>
        private async Task<float> CalculateCompletionPercentage(int ordreNr)
        {
            var sjekklisteSjekkpunkter = await _context.SjekklisteSjekkpunkt
                .Include(ss => ss.sjekkpunkt)
                .ThenInclude(s => s.Kategori)
                .Where(sl => sl.OrdreNr == ordreNr)
                .ToListAsync();

            if (!sjekklisteSjekkpunkter.Any())
            {
                return 0; // No checkpoints for the order
            }

            var groupedSjekkpunkter = sjekklisteSjekkpunkter
                .GroupBy(ss => ss.sjekkpunkt.Kategori.KategoriNavn)
                .ToList();

            int totalCategories = groupedSjekkpunkter.Count;
            int completedCategories = 0;

            foreach (var category in groupedSjekkpunkter)
            {
                if (category.All(s => !string.IsNullOrEmpty(s.Status)))
                {
                    completedCategories++; // Increment if all checkpoints in the category have a status
                }
            }

            // Calculate percentage and round to nearest whole number
            float completionPercentage = (float)Math.Round((completedCategories / (float)totalCategories) * 100, 0);
            return completionPercentage;
        }




        
        // Public Methods ------------------------------------------------------------
        
         /// <summary>
         /// Displays a list of orders with their completion percentages.
         /// </summary>
         /// <returns>A view displaying the list of orders.</returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var ordreList = await _context.Ordre1.ToListAsync();
            var viewModel = new List<OrdreCompletionViewModel>();

            foreach (var ordre in ordreList)
            {
                var completionPercentage = await CalculateCompletionPercentage(ordre.OrdreNr);
                viewModel.Add(new OrdreCompletionViewModel
                {
                    Ordre = ordre,
                    CompletionPercentage = completionPercentage
                });
            }

            return View(viewModel);
        }
        
        /// <summary>
        /// Displays details of a specific order, including its completion percentage.
        /// </summary>
        /// <param name="id">The ID of the order to display.</param>
        /// <returns>A view displaying the order details.</returns>
        public async Task<IActionResult> Details(int? id)
        {
            {
                var result = await FetchOrdreViewModelOrNotFoundAsync(id, "OrdreCompletionViewModel");
                if (result is ViewResult viewResult && viewResult.Model is OrdreEntity ordreViewModel)
                {
                    var completionPercentage = await CalculateCompletionPercentage(id.Value);
                    var viewModel = new OrdreCompletionViewModel
                    {
                        Ordre = ordreViewModel,
                        CompletionPercentage = completionPercentage
                    };

                    return View("Details", viewModel); // Change to the appropriate view name
                }

                return result;
            }
        }

        
        /// <summary>
        /// Displays a form to create a new order.
        /// </summary>
        /// <returns>A view displaying the order creation form.</returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        
        /// <summary>
        /// Handles the creation of a new order.
        /// </summary>
        /// <param name="ordreEntity">The order view model to create.</param>
        /// <returns>Redirects to the order list if successful; otherwise, returns the creation form.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrdreNr,Navn,TelefonNr,Adresse,Type,Gjelder,Epost,Uke,Registrert,Bestilling,AvtaltLevering,ProduktMotatt,AvtaltFerdigstillelse,ServiceFerdig,AntallTimer,Status")] OrdreEntity ordreEntity)
        {
            ModelState.Remove("OrdreNr");
            if (ModelState.IsValid)
            {
                _context.Add(ordreEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ordreEntity);
        }
        
        /// <summary>
        /// Displays a form to edit an existing order.
        /// </summary>
        /// <param name="id">The ID of the order to edit.</param>
        /// <returns>A view displaying the order editing form.</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            return await FetchOrdreViewModelOrNotFoundAsync(id, "Edit"); 
        }


        /// <summary>
        /// Handles the editing of an existing order.
        /// </summary>
        /// <param name="id">The ID of the order to edit.</param>
        /// <param name="ordreEntity">The updated order view model.</param>
        /// <returns>Redirects to the order list if successful; otherwise, returns the editing form.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrdreNr,Navn,TelefonNr,Adresse,Type,Gjelder,Epost,Uke,Registrert,Bestilling,AvtaltLevering,ProduktMotatt,AvtaltFerdigstillelse,ServiceFerdig,AntallTimer,status")] OrdreEntity ordreEntity)
        {
            if (id != ordreEntity.OrdreNr)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ordreEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdreViewModelExists(ordreEntity.OrdreNr))
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
            return View(ordreEntity);
        }
        
        /// <summary>
        /// Displays details of a specific order for deletion.
        /// </summary>
        /// <param name="id">The ID of the order to delete.</param>
        /// <returns>A view displaying the order details for deletion.</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
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

        
        /// <summary>
        /// Handles the deletion of a specific order.
        /// </summary>
        /// <param name="id">The ID of the order to delete.</param>
        /// <returns>Redirects to the order list if successful; otherwise, returns an error.</returns>
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
      

    }
}