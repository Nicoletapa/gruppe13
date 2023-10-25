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
              return _context.SjekklisteViewModel != null ? 
                          View(await _context.SjekklisteViewModel.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.SjekklisteViewModels'  is null.");
        }

        // GET: Sjekkliste/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SjekklisteViewModel == null)
            {
                return NotFound();
            }

            var sjekklisteViewModel = await _context.SjekklisteViewModel
                .FirstOrDefaultAsync(m => m.SjekklisteID == id);
            if (sjekklisteViewModel == null)
            {
                return NotFound();
            }

            return View(sjekklisteViewModel);
        }

        // GET: Sjekkliste/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sjekkliste/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SjekklisteID, sjekklisteMekanisk, sjekklisteHydraulisk, sjekklisteElektro, sjekklisteTrykkSettinger, SjekklisteFunksjonsTests, sjekklisteKommentarer")] SjekklisteViewModel sjekklisteViewModel)
        {
             if (ModelState.IsValid)
            {
             var sjekklisteViewModel1 = new SjekklisteViewModel()
                            {
                                sjekklisteMekanisk = new List<SjekklisteMekanisk>(),
                                sjekklisteHydraulisk = new List<SjekklisteHydraulisk>(),
                                sjekklisteElektro = new List<SjekklisteElektro>(),
                                sjekklisteTrykkSettinger = new List<SjekklisteTrykkSettinger>(),
                                SjekklisteFunksjonsTests = new List<SjekklisteFunksjonsTest>(),
                                sjekklisteKommentarer = new List<SjekklisteKommentarer>()
                                
                            };
                
                _context.Add(sjekklisteViewModel1);

                // Save changes to the database.
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
           
                 }
             
             
            // If the ModelState is not valid, return to the Create view.
           return View(sjekklisteViewModel);
        }*/
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SjekklisteID, sjekklisteMekanisk, sjekklisteHydraulisk, SjekklisteFunksjonsTests, sjekklisteElektro, sjekklisteTrykkSettinger ")]SjekklisteViewModel sjekklisteViewModel)
        {
          //  Remove ModelState errors for the lists that you're handling separately
            ModelState.Remove("sjekklisteMekanisk");
            ModelState.Remove("sjekklisteHydraulisk");
            ModelState.Remove("sjekklisteElektro");
            ModelState.Remove("sjekklisteTrykkSettinger");
            ModelState.Remove("SjekklisteFunksjonsTests");
            ModelState.Remove("sjekklisteKommentarer");
            // ModelState.Remove("SjekklisteMekaniskID");
            // ModelState.Remove("SjekklisteHydrauliskID");
            // ModelState.Remove("SjekklisteElektroID");
            // ModelState.Remove("SjekklisteFunksjonsTestID");
            // ModelState.Remove("SjekklisteTrykkSettingerID");
            // ModelState.Remove("SjekklisteKommentarerID");
             

            if (ModelState.IsValid)
            {
                try
                {

                    var mekanisk = new SjekklisteMekanisk
                    {
                        
                        SjekkClutchLamellerForSlitasje = Request.Form["SjekkClutchLamellerForSlitasje"],
                        SjekkBremserBåndPål = Request.Form["SjekkBremserBåndPål"],
                        SjekkLagerForTrommel = Request.Form["SjekkLagerForTrommel"],
                        SjekkPTOOgOpplagring = Request.Form["SjekkPTOgOppagring"],
                        SjekkWire = Request.Form["SjekkWire"],
                        SjekkPinionLager = Request.Form["SjekkPinionLager"],
                        SjekkKjedeStrammer = Request.Form["SjekkKjedeStrammer"],
                        SjekkKilePåKjedehjul = Request.Form["SjekkKilePåKjedehjul"]
                    };

                   

                    var hydraulisk = new SjekklisteHydraulisk
                    {
                        SkiftOljeITank = Request.Form["SkiftOljeITank"],
                        SjekkHydraulikkSylinderForLekkasje = Request.Form["SjekkHydraulikkSylinderForLekkasje"],
                        SkiftOljePåGirBoks = Request.Form["SkiftOljePåGirBoks"],
                        SjekkRingsylinderÅpneOgSkiftTetninger = Request.Form["SjekkRingsylinderÅpneOgSkiftTetninger"],
                        SjekkSlangerForSkaderOgLekkasje = Request.Form["SjekkSlangerForSkaderOgLekkasje"],
                        SjekkBremseSylinderÅpneOgSkiftTetninger = Request.Form["SjekkBremseSylinderÅpneOgSkiftTetninger"],
                        TestHydraulikkBlokkITestbenk = Request.Form["TestHydraulikkBlokkITestbenk"]

                    };

                    var elektro = new SjekklisteElektro
                    {
                        SjekkLedningsnettPåVinsj = Request.Form["SjekkLedningsnettPåVinsj"],
                        SjekkOgTestKnappekasse = Request.Form["SjekkOgTestKnappekasse"],
                        SjekkOgTestRadio = Request.Form["SjekkOgTestRadio"]
                    };
                    
                    var trykkSettinger = new SjekklisteTrykkSettinger
                    {
                        xx_Bar = Request.Form["xx_Bar"]

                    };

                    var funksjonsTest = new SjekklisteFunksjonsTest
                    {
                        TestVinsjOgKjørAlleFunksjoner = Request.Form["TestVinsjOgKjørAlleFunksjoner"],
                        BremseKraftKN = Request.Form["BremseKraftKN"],
                        TrekkkraftKN = Request.Form["TrekkkraftKN"]

                    };
                    var kommentarer = new SjekklisteKommentarer
                    {
                        Kommentar = Request.Form["Kommentar"]
                    };

                    sjekklisteViewModel.sjekklisteMekanisk.Add(mekanisk);
                    sjekklisteViewModel.sjekklisteHydraulisk.Add(hydraulisk);
                    sjekklisteViewModel.sjekklisteElektro.Add(elektro);
                    sjekklisteViewModel.sjekklisteTrykkSettinger.Add(trykkSettinger);
                    sjekklisteViewModel.SjekklisteFunksjonsTests.Add(funksjonsTest);
                    sjekklisteViewModel.sjekklisteKommentarer.Add(kommentarer);
                    
                    

                    // Add your sjekklisteViewModel to the context
                    _context.Add(sjekklisteViewModel);

                    // Save changes to the database
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that might occur during data processing.
                    ModelState.AddModelError(string.Empty, "An error occurred while saving data: " + ex.Message);
                }
            }

            // If the ModelState is not valid, return to the Create view.
            return View(sjekklisteViewModel);
        }


        


        // GET: Sjekkliste/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SjekklisteViewModel == null)
            {
                return NotFound();
            }

            var sjekklisteViewModel = await _context.SjekklisteViewModel.FindAsync(id);
            if (sjekklisteViewModel == null)
            {
                return NotFound();
            }
            return View(sjekklisteViewModel);
        }

        // POST: Sjekkliste/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SjekklisteID")] SjekklisteViewModel sjekklisteViewModel)
        {
            if (id != sjekklisteViewModel.SjekklisteID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sjekklisteViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SjekklisteViewModelExists(sjekklisteViewModel.SjekklisteID))
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
            return View(sjekklisteViewModel);
        }

        // GET: Sjekkliste/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SjekklisteViewModel == null)
            {
                return NotFound();
            }

            var sjekklisteViewModel = await _context.SjekklisteViewModel
                .FirstOrDefaultAsync(m => m.SjekklisteID == id);
            if (sjekklisteViewModel == null)
            {
                return NotFound();
            }

            return View(sjekklisteViewModel);
        }

        // POST: Sjekkliste/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SjekklisteViewModel == null)
            {
                return Problem("Entity set 'ApplicationDbContext.SjekklisteViewModels'  is null.");
            }
            var sjekklisteViewModel = await _context.SjekklisteViewModel.FindAsync(id);
            if (sjekklisteViewModel != null)
            {
                _context.SjekklisteViewModel.Remove(sjekklisteViewModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SjekklisteViewModelExists(int id)
        {
          return (_context.SjekklisteViewModel?.Any(e => e.SjekklisteID == id)).GetValueOrDefault();
        }
    }
}
