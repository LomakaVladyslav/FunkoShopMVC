using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FunkoShopDomain.Model;
using FunkoShopInfrastructure;

namespace FunkoShopInfrastructure.Controllers
{
    public class FiguresController : Controller
    {
        private readonly FunkoShopContext _context;

        public FiguresController(FunkoShopContext context)
        {
            _context = context;
        }

        // GET: Figures
        public async Task<IActionResult> Index()
        {
            var funkoShopContext = _context.Figures.Include(f => f.Category).Include(f => f.Country);
            return View(await funkoShopContext.ToListAsync());
        }

        // GET: Figures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var figure = await _context.Figures
                .Include(f => f.Category)
                .Include(f => f.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (figure == null)
            {
                return NotFound();
            }

            return View(figure);
        }

        // GET: Figures/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Code");
            return View();
        }

        // POST: Figures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,ImageUrl,CategoryId,CountryId")] Figure figure)
        {
            if (ModelState.IsValid)
            {
                _context.Add(figure);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", figure.CategoryId);
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Code", figure.CountryId);
            return View(figure);
        }

        // GET: Figures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var figure = await _context.Figures.FindAsync(id);
            if (figure == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", figure.CategoryId);
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Code", figure.CountryId);
            return View(figure);
        }

        // POST: Figures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,ImageUrl,CategoryId,CountryId")] Figure figure)
        {
            if (id != figure.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(figure);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FigureExists(figure.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", figure.CategoryId);
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Code", figure.CountryId);
            return View(figure);
        }

        // GET: Figures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var figure = await _context.Figures
                .Include(f => f.Category)
                .Include(f => f.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (figure == null)
            {
                return NotFound();
            }

            return View(figure);
        }

        // POST: Figures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var figure = await _context.Figures.FindAsync(id);
            if (figure != null)
            {
                _context.Figures.Remove(figure);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FigureExists(int id)
        {
            return _context.Figures.Any(e => e.Id == id);
        }
    }
}
