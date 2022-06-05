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
    public class HerbsController : Controller
    {
        private readonly _3PoisonAPIContext _context;

        public HerbsController(_3PoisonAPIContext context)
        {
            _context = context;
        }

        // GET: Herbs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Herbs.ToListAsync());
        }


        // GET: Herbs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Herbs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Herb qw)
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

        // GET: Herbs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qw = await _context.Herbs.FindAsync(id);
            if (qw == null)
            {
                return NotFound();
            }
            return View(qw);
        }

        // POST: Herbs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Herb qw)
        {
            if (id != qw.Id)
            {
                return NotFound();
            }
            var model = _context.Herbs.FirstOrDefault(a => a.Name.Equals(qw.Name) && a.Id != id);
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
                        if (!HerbExists(qw.Id))
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

        // GET: Herbs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qw = await _context.Herbs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (qw == null)
            {
                return NotFound();
            }

            return View(qw);
        }

        // POST: Herbs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var qw = await _context.Herbs.FindAsync(id);
            var filmHerbRelationships = _context.Herbs_Ps.Where(r => r.HerbId == id).ToList();
            foreach (var item in filmHerbRelationships)
            {
                _context.Herbs_Ps.Remove(item);
                await _context.SaveChangesAsync();
            }
            _context.Herbs.Remove(qw);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HerbExists(int id)
        {
            return _context.Herbs.Any(e => e.Id == id);
        }
        private bool IsDuplicate(Herb model)
        {
            var qw = _context.Herbs.FirstOrDefault(a => a.Name.Equals(model.Name));

            return qw == null ? false : true;
        }
    }
}