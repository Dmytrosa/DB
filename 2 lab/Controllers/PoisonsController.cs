using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DB_lab2;

namespace DB_lab2.Controllers
{
    public class PoisonsController : Controller
    {
        private readonly _3PoisonAPIContext _context;

        public PoisonsController(_3PoisonAPIContext context)
        {
            _context = context;
        }

        // GET: Poisons
        public async Task<IActionResult> Index()
        {
            return View(await _context.Poisons.ToListAsync());
        }



        // GET: Poisons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Poisons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Poison poi)
        {
            if (!IsDuplicate(poi))
            {
                if (ModelState.IsValid)
                {
                    _context.Add(poi);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(poi);
            }
            else
                ModelState.AddModelError("Name", "Такий жанр уже існує");

            return View(poi);
        }

        // GET: Poisons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var poi = await _context.Poisons.FindAsync(id);
            if (poi == null)
            {
                return NotFound();
            }
            return View(poi);
        }

        // POST: Poisons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Poison poi)
        {
            if (id != poi.Id)
            {
                return NotFound();
            }
            var model = _context.Poisons.FirstOrDefault(g => g.Name.Equals(poi.Name) && g.Id != id);
            if (model == null)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(poi);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!PoisonExists(poi.Id))
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
                return View(poi);
            }
            else
                ModelState.AddModelError("Name", "Такий жанр уже існує");

            return View(poi);
        }

        // GET: Poisons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var poi = await _context.Poisons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (poi == null)
            {
                return NotFound();
            }

            return View(poi);
        }

        // POST: Poisons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var poi = await _context.Poisons.FindAsync(id);
            var filmPoisonRelationships = _context.PPs.Where(r => r.PoisonId == id).ToList();
            foreach (var item in filmPoisonRelationships)
            {
                _context.PPs.Remove(item);
                await _context.SaveChangesAsync();
            }
            _context.Poisons.Remove(poi);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PoisonExists(int id)
        {
            return _context.Poisons.Any(e => e.Id == id);
        }
        private bool IsDuplicate(Poison model)
        {
            var poi = _context.Poisons.FirstOrDefault(g => g.Name.Equals(model.Name));

            return poi == null ? false : true;
        }
    }
}
