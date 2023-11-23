using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using spbu.Data;
using spbu.Models;

namespace spbu.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly ApplicationDbContext _context;

    private readonly int _limitBbm = 100;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.DataKendaraan.ToListAsync());
    }

    public async Task<IActionResult> Details(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var DataKendaraan = _context.DataKendaraan
            .FirstOrDefault(m => m.Id == id);
        if (DataKendaraan == null)
        {
            return NotFound();
        }

        return View(DataKendaraan);
    }

    // GET: Home/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Home/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DataKendaraan DataKendaraan)
    {
        var username = "admin";// User.Identity.Name;
        var dtnow = DateTime.Now;

        if (ModelState.IsValid)
        {
            DataKendaraan.TanggalPengisian = DateTime.Now.Date;
            DataKendaraan.Created = dtnow;
            DataKendaraan.Modified = dtnow;
            DataKendaraan.Createdby = username;
            DataKendaraan.Modifiedby = username;
            _context.Add(DataKendaraan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(DataKendaraan);
    }


    // GET: Home/CheckData
    public IActionResult CheckData()
    {
        return View();
    }

    // POST: Home/CheckData
    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckData(CheckDataViewModel DataKendaraan)
    {
        // var username = "admin";// User.Identity.Name;
        // var dtnow = DateTime.Now;

        if (ModelState.IsValid)
        {
            DataKendaraan.TanggalPengisian = DateTime.Now.Date;

            var list = await _context.DataKendaraan
            .Where(x => x.NomorPlat == DataKendaraan.NomorPlat
            && x.TanggalPengisian == DataKendaraan.TanggalPengisian).ToListAsync();

            if (list.Count > 0)
            {
                DataKendaraan.TotalBbm = list.Sum(x => x.JumlahBbm);
            }

            DataKendaraan.LimitBbm = _limitBbm;
            DataKendaraan.SisaBbm = DataKendaraan.LimitBbm - DataKendaraan.TotalBbm;

            return View(DataKendaraan);
        }
        return View(DataKendaraan);
    }

    // GET: Home/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var DataKendaraan = await _context.DataKendaraan.FindAsync(id);
        if (DataKendaraan == null)
        {
            return NotFound();
        }
        return View(DataKendaraan);
    }

    // POST: Home/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, DataKendaraan DataKendaraan)
    {
        if (id != DataKendaraan.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                DataKendaraan.Modified = DateTime.Now;
                DataKendaraan.Modifiedby = "admin"; // User.Identity.Name;
                DataKendaraan.TanggalPengisian = DateTime.Now.Date;
                _context.Update(DataKendaraan);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DataKendaraanExists(DataKendaraan.Id))
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
        return View(DataKendaraan);
    }

    // GET: Home/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var DataKendaraan = await _context.DataKendaraan
            .FirstOrDefaultAsync(m => m.Id == id);
        if (DataKendaraan == null)
        {
            return NotFound();
        }

        return View(DataKendaraan);
    }

    // POST: Home/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var DataKendaraan = await _context.DataKendaraan.FindAsync(id);
        _context.DataKendaraan.Remove(DataKendaraan);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool DataKendaraanExists(string id)
    {
        return _context.DataKendaraan.Any(e => e.Id == id);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
