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
    public class PoisonersController : Controller
    {
        private readonly _3PoisonAPIContext _context;

        public PoisonersController(_3PoisonAPIContext context)
        {
            _context = context;
        }

        // GET: Poisoners

        private void FillReturnPath(string? returnController)
        {
            ViewBag.ReturnController = returnController;
        }
        private void FillSelectLists(int id)
        {
            ViewBag.AddressList = new SelectList(_context.Addresses, "Id", "Name");
            SelectList ganres = new SelectList(_context.Poisons, "Id", "Name");
            ViewBag.PoisonList = new List<SelectListItem>();
            PP r = new PP();

            foreach (var g in ganres)
            {
                ViewBag.PoisonList.Add(new SelectListItem { Value = g.Value, Text = g.Text });
            }
            SelectList actors = new SelectList(_context.Herbalists, "Id", "Name");
            ViewBag.HerbalistList = new List<SelectListItem>();
            foreach (var a in actors)
            {
                ViewBag.HerbalistList.Add(new SelectListItem { Value = a.Value, Text = a.Text });
            }
            ViewBag.Id = id;

        }
        public async Task<IActionResult> Index(int firstId, string? retController)
        {
            FillReturnPath(retController);
            FillSelectLists(firstId);
            if (retController == "Addresses")
            {
                var filmsByAddress = _context.Poisoners.Where(f => f.AddressId == firstId)
                    .Include(f => f.Address)
                    .Include(f => f.PPs)
                    .ThenInclude(f => f.Poison)
                    .Include(f => f.HPs)
                    .ThenInclude(f => f.Herbalist);
                return View(await filmsByAddress.ToListAsync());
            }
            else if (retController == "Poisons")
            {
                var filmsIds = _context.PPs.Where(r => r.PoisonId == firstId).Select(r => r.PoisonerId).ToList();
                var filmsByPoison = _context.Poisoners.Where(f => filmsIds.Contains(f.Id))
                    .Include(f => f.Address)
                    .Include(f => f.PPs)
                    .ThenInclude(f => f.Poison)
                    .Include(f => f.HPs)
                    .ThenInclude(f => f.Herbalist);
                return View(await filmsByPoison.ToListAsync());
            }
            else if (retController == "Herbalists")
            {
                var filmsIds = _context.HPs.Where(r => r.HerbalistId == firstId).Select(r => r.PoisonerId).ToList();
                var filmsByHerbalist = _context.Poisoners.Where(f => filmsIds.Contains(f.Id))
                    .Include(f => f.Address)
                    .Include(f => f.PPs)
                    .ThenInclude(f => f.Poison)
                    .Include(f => f.HPs)
                    .ThenInclude(f => f.Herbalist);
                return View(await filmsByHerbalist.ToListAsync());
            }
            else
            {
                return View(await _context.Poisoners.Include(f => f.Address)
                    .Include(f => f.PPs)
                    .ThenInclude(f => f.Poison)
                    .Include(f => f.HPs)
                    .ThenInclude(f => f.Herbalist).ToListAsync());
            }
        }





        // GET: Poisoners/Create
        public IActionResult Create(int firstId, string? retController)
        {
            FillSelectLists(firstId);
            FillReturnPath(retController);
            //ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Name");
            //ViewBag.AddressId = categoryId;
            //ViewBag.AddressName = _context.Addresses.Where(c => c.Id == categoryId).FirstOrDefault().Name;
            return View();
        }

        // POST: Poisoners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,AddressId,BirthDate,Info")] Poisoner film, int firstId, string? retController)
        {
            FillSelectLists(firstId);

            FillReturnPath(retController);
            var PoId = Request.Form["ganres"];
            var HerID = Request.Form["actors"];
            List<int> gIds = new List<int>();
            foreach (var g in PoId)
            {
                gIds.Add(int.Parse(g));
            }
            List<int> aIds = new List<int>();
            foreach (var a in HerID)
            {
                aIds.Add(int.Parse(a));
            }
            if (!IsDuplicate(film, gIds, aIds, 0))
            {
                if (ModelState.IsValid)
                {
                    _context.Add(film);

                    foreach (var gId in gIds)
                    {
                        var poi = _context.Poisons.Where(g => g.Id == gId).FirstOrDefault();
                        PP fgr = new PP();
                        fgr.Poisoner = film;
                        fgr.PoisonId = gId;
                        fgr.Poison = poi;
                        poi.PPs.Add(fgr);
                        film.PPs.Add(fgr);
                        _context.PPs.Add(fgr);
                    }

                    foreach (var aId in aIds)
                    {
                        var actor = _context.Herbalists.Where(a => a.Id == aId).FirstOrDefault();
                        HP far = new HP();
                        far.Poisoner = film;
                        far.HerbalistId = aId;
                        far.Herbalist = actor;
                        actor.HPs.Add(far);
                        film.HPs.Add(far);
                        _context.HPs.Add(far);
                    }
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Poisoners", new { firstId = firstId, retController = retController });
                }
                return View(film);
            }
            else
                ModelState.AddModelError("", "Такий фільм уже існує");

            return View(film);
        }

        public async Task<IActionResult> Edit(int? id, int firstId, string? retController)
        {
            FillSelectLists(firstId);
            FillReturnPath(retController);
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Poisoners.FindAsync(id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // POST: Poisoners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Name,AddressId,BirthDate,Info")] Poisoner film, int id, int firstId, string? retController)
        {
            FillSelectLists(firstId);
            FillReturnPath(retController);
            var PoId = Request.Form["ganres"];
            var HerID = Request.Form["actors"];
            List<int> gIds = new List<int>();
            foreach (var g in PoId)
            {
                gIds.Add(int.Parse(g));
            }
            List<int> aIds = new List<int>();
            foreach (var a in HerID)
            {
                aIds.Add(int.Parse(a));
            }
            if (!IsDuplicate(film, gIds, aIds, id))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(film);
                        film.PPs.Clear();
                        var GIds = _context.PPs.Where(r => r.PoisonerId == id);
                        foreach (var item in GIds)
                        {
                            _context.Remove(item);
                        }
                        foreach (var gId in gIds)
                        {
                            var poi = _context.Poisons.Where(g => g.Id == gId).FirstOrDefault();
                            PP fgr = new PP();
                            fgr.Poisoner = film;
                            fgr.PoisonId = gId;
                            fgr.Poison = poi;
                            foreach (var e in poi.PPs)
                            {
                                if (e.PoisonerId == film.Id) poi.PPs.Remove(e);
                            }
                            poi.PPs.Add(fgr);
                            film.PPs.Add(fgr);
                            _context.PPs.Add(fgr);
                        }
                        film.PPs.Clear();
                        var AIds = _context.HPs.Where(r => r.PoisonerId == id);
                        foreach (var item in AIds)
                        {
                            _context.Remove(item);
                        }
                        foreach (var aId in aIds)
                        {
                            var actor = _context.Herbalists.Where(a => a.Id == aId).FirstOrDefault();
                            HP far = new HP();
                            far.Poisoner = film;
                            far.HerbalistId = aId;
                            far.Herbalist = actor;
                            foreach (var e in actor.HPs)
                            {
                                if (e.PoisonerId == film.Id) actor.HPs.Remove(e);
                            }
                            actor.HPs.Add(far);
                            film.HPs.Add(far);
                            _context.HPs.Add(far);
                        }
                        await _context.SaveChangesAsync();

                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!PoisonerExists(film.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction("Index", "Poisoners", new { firstId = firstId, retController = retController });
                }
                return View(film);
            }
            else
                ModelState.AddModelError("", "Такий фільм уже існує");

            return View(film);
        }

        // GET: Poisoners/Delete/5
        public async Task<IActionResult> Delete(int id, int firstId, string? retController)
        {
            FillSelectLists(firstId);
            FillReturnPath(retController);


            var film = await _context.Poisoners
                .Include(f => f.Address)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // POST: Poisoners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int firstId, string? retController)
        {
            FillSelectLists(firstId);
            FillReturnPath(retController);
            var film = await _context.Poisoners.FindAsync(id);
            var ganresIds = _context.PPs.Where(g => g.PoisonerId == id);
            foreach (var item in ganresIds)
            {
                _context.PPs.Remove(item);
            }

            var actorsIds = _context.HPs.Where(g => g.PoisonerId == id);
            foreach (var item in actorsIds)
            {
                _context.HPs.Remove(item);
            }
            _context.Poisoners.Remove(film);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Poisoners", new { firstId = firstId, retController = retController });
        }

        private bool PoisonerExists(int id)
        {
            return _context.Poisoners.Any(e => e.Id == id);
        }
        private bool IsDuplicate(Poisoner model, List<int> gIds, List<int> aIds, int id)
        {


            var films = _context.Poisoners.Where(f => f.Name.Equals(model.Name)
                                               && f.BirthDate.Equals(model.BirthDate)
                                               && f.AddressId.Equals(model.AddressId)
                                               && f.Id != id)
                                            .Include(f => f.Address)
                                            .Include(f => f.PPs)
                                            .ThenInclude(f => f.Poison)
                                            .Include(f => f.HPs)
                                            .ThenInclude(f => f.Herbalist);
            foreach (var f in films)
            {
                foreach (var fg in f.PPs)
                {
                    if (!gIds.Contains(fg.PoisonId)) return true;
                }
                foreach (var fa in f.HPs)
                {
                    if (!aIds.Contains(fa.HerbalistId)) return true;
                }
                if (f.Info.Equals(model.Info)) return true;
            }
            return false;
        }
    }
}
