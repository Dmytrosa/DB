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
    public class HerbalistsController : Controller
    {
        private readonly _3PoisonAPIContext _context;

        public HerbalistsController(_3PoisonAPIContext context)
        {
            _context = context;
        }

        // GET: Herbalists
        public async Task<IActionResult> Index()
        {
            return View(await _context.Herbalists.ToListAsync());
        }


        // GET: Herbalists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Herbalists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Herbalist qw)
        {
            if (!IsDuplicate(qw))
            {

                if (ModelState.IsValid)
                {
                    _context.Add(qw);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                return View(qw);
            }
            else
                ModelState.AddModelError("Name", "Такий актор уже існує");

            return View(qw);
        }

        // GET: Herbalists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qw = await _context.Herbalists.FindAsync(id);
            if (qw == null)
            {
                return NotFound();
            }
            return View(qw);
        }

        // POST: Herbalists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Herbalist qw)
        {
            if (id != qw.Id)
            {
                return NotFound();
            }
            var model = _context.Herbalists.FirstOrDefault(a => a.Name.Equals(qw.Name) && a.Id != id);
            if (model == null)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(qw);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!HerbalistExists(qw.Id))
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
                return View(qw);
            }
            else
                ModelState.AddModelError("Name", "Такий актор уже існує");

            return View(qw);
        }

        // GET: Herbalists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qw = await _context.Herbalists
                .FirstOrDefaultAsync(m => m.Id == id);
            if (qw == null)
            {
                return NotFound();
            }

            return View(qw);
        }

        // POST: Herbalists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var qw = await _context.Herbalists.FindAsync(id);
            var filmHerbalistRelationships = _context.HPs.Where(r => r.HerbalistId == id).ToList();
            foreach (var item in filmHerbalistRelationships)
            {
                _context.HPs.Remove(item);
                await _context.SaveChangesAsync();
            }
            _context.Herbalists.Remove(qw);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HerbalistExists(int id)
        {
            return _context.Herbalists.Any(e => e.Id == id);
        }
        private bool IsDuplicate(Herbalist model)
        {
            var qw = _context.Herbalists.FirstOrDefault(a => a.Name.Equals(model.Name));

            return qw == null ? false : true;
        }
    }
}
