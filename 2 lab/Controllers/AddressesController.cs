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
    public class AddressesController : Controller
    {
        private readonly _3PoisonAPIContext _context;

        public AddressesController(_3PoisonAPIContext context)
        {
            _context = context;
        }

        // GET: Addresses
        public async Task<IActionResult> Index()
        {
            ViewBag.AddressesList = new SelectList(_context.Addresses, "Id", "Name");
            return View(await _context.Addresses.ToListAsync());
        }


        // GET: Addresses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Addresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Address category)
        {
            if (!IsDuplicate(category))
            {
                ModelState.AddModelError("Name", "Така категорія уже існує");

                if (ModelState.IsValid)
                {
                    _context.Add(category);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(category);
            }
            else
                ModelState.AddModelError("Name", "Така категорія уже існує");

            return View(category);
        }

        // GET: Addresses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Addresses.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Addresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Address category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }
            var model = _context.Addresses.FirstOrDefault(c => c.Name.Equals(category.Name) && c.Id != id);
            if (model == null)
            {

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(category);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!AddressExists(category.Id))
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
                return View(category);
            }
            else
                ModelState.AddModelError("Name", "Така категорія уже існує");
            return View(category);
        }

        // GET: Addresses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Addresses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Addresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Addresses.FindAsync(id);
            var films = _context.Poisoners.Where(f => f.AddressId == id).ToList();
            if (films.Count == 0)
            {
                _context.Addresses.Remove(category);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));

        }

        private bool AddressExists(int id)
        {
            return _context.Addresses.Any(e => e.Id == id);
        }
        private bool IsDuplicate(Address model)
        {
            var cat = _context.Addresses.FirstOrDefault(c => c.Name.Equals(model.Name));

            return cat == null ? false : true;
        }
    }
}
