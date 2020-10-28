using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Bardcore.Models;
using Bardcore.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using WebMatrix.Data;

namespace Bardcore.Controllers
{
    public class MusicController : Controller
    {
        private readonly BardcoreContext _context;
        private IHostingEnvironment _webroot;

        public MusicController(BardcoreContext context, IHostingEnvironment webroot)
        {
            _context = context;
            _webroot = webroot;
        }

        // GET: Music
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            var bardcoreContext = _context.SongInfo.Include(s => s.GenreNavigation); 
            return View(await bardcoreContext.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> Browse()
        {
            var bardcoreContext = _context.SongInfo.Include(s => s.GenreNavigation);
            return View(await bardcoreContext.ToListAsync());

        }

        // GET: Music/Details/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var songInfo = await _context.SongInfo
                .Include(s => s.GenreNavigation)
                .FirstOrDefaultAsync(m => m.TrackId == id);
            if (songInfo == null)
            {
                return NotFound();
            }

            return View(songInfo);
        }

        // GET: Music/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["Genre"] = new SelectList(_context.Genre, "GenreId", "Gname");
            return View();
        }

        // POST: Music/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrackId,Name,Artist,Genre,ReleaseYear,UploadDate,FileLocation")] SongInfo songInfo,
            IFormFile FileMusic)
        {

            if (FileMusic.Length > 0)
            {
                string FileLocation = _webroot.WebRootPath + "\\uploads\\";
                var fileName = Path.GetFileName(FileMusic.FileName);

                using (var stream = System.IO.File.Create(FileLocation + fileName))
                {
                    await FileMusic.CopyToAsync(stream);
                    songInfo.FileLocation = fileName;
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(songInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Genre"] = new SelectList(_context.Genre, "GenreId", "Gname", songInfo.Genre);
            return View(songInfo);
        }

        // GET: Music/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var songInfo = await _context.SongInfo.FindAsync(id);
            if (songInfo == null)
            {
                return NotFound();
            }
            ViewData["Genre"] = new SelectList(_context.Genre, "GenreId", "Gname", songInfo.Genre);
            return View(songInfo);
        }

        // POST: Music/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("TrackId,Name,Artist,Genre,ReleaseYear,UploadDate,FileLocation")] SongInfo songInfo)
        {
            if (id != songInfo.TrackId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(songInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SongInfoExists(songInfo.TrackId))
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
            ViewData["Genre"] = new SelectList(_context.Genre, "GenreId", "Gname", songInfo.Genre);
            return View(songInfo);
        }

        // GET: Music/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var songInfo = await _context.SongInfo
                .Include(s => s.GenreNavigation)
                .FirstOrDefaultAsync(m => m.TrackId == id);
            if (songInfo == null)
            {
                return NotFound();
            }

            return View(songInfo);
        }

        // POST: Music/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var songInfo = await _context.SongInfo.FindAsync(id);
            _context.SongInfo.Remove(songInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SongInfoExists(int id)
        {
            return _context.SongInfo.Any(e => e.TrackId == id);
        }
    }
}
