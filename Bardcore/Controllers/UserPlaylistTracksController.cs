using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bardcore.Models;

namespace Bardcore.Controllers
{
    public class UserPlaylistTracksController : Controller
    {
        private readonly BardcoreContext _context;

        public UserPlaylistTracksController(BardcoreContext context)
        {
            _context = context;
        }

        // GET: UserPlaylistTracks
        public async Task<IActionResult> Index()
        {
            var bardcoreContext = _context.UserPlaylistTrack.Include(u => u.Playlist).Include(u => u.Track);
            return View(await bardcoreContext.ToListAsync());
        }

        // GET: UserPlaylistTracks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userPlaylistTrack = await _context.UserPlaylistTrack
                .Include(u => u.Playlist)
                .Include(u => u.Track)
                .FirstOrDefaultAsync(m => m.PlaylistTrackId == id);
            if (userPlaylistTrack == null)
            {
                return NotFound();
            }

            return View(userPlaylistTrack);
        }

        // GET: UserPlaylistTracks/Create
        public IActionResult Create()
        {
            ViewData["PlaylistId"] = new SelectList(_context.UserPlaylist, "PlaylistId", "PlaylistName");
            ViewData["TrackId"] = new SelectList(_context.SongInfo, "TrackId", "Name");
            return View();
        }

        // POST: UserPlaylistTracks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlaylistTrackId,PlaylistId,TrackId")] UserPlaylistTrack userPlaylistTrack)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userPlaylistTrack);
                await _context.SaveChangesAsync();
                return RedirectToAction("Browse", "UserPlaylists");
            }
            ViewData["PlaylistId"] = new SelectList(_context.UserPlaylist, "PlaylistId", "PlaylistName", userPlaylistTrack.PlaylistId);
            ViewData["TrackId"] = new SelectList(_context.SongInfo, "TrackId", "Name", userPlaylistTrack.TrackId);
            return View("~/Views/UserPlaylists/Browse.cshtml");
        }

        // GET: UserPlaylistTracks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userPlaylistTrack = await _context.UserPlaylistTrack.FindAsync(id);
            if (userPlaylistTrack == null)
            {
                return NotFound();
            }
            ViewData["PlaylistId"] = new SelectList(_context.UserPlaylist, "PlaylistId", "PlaylistName", userPlaylistTrack.PlaylistId);
            ViewData["TrackId"] = new SelectList(_context.SongInfo, "TrackId", "Name", userPlaylistTrack.TrackId);
            return View(userPlaylistTrack);
        }

        // POST: UserPlaylistTracks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PlaylistTrackId,PlaylistId,TrackId")] UserPlaylistTrack userPlaylistTrack)
        {
            if (id != userPlaylistTrack.PlaylistTrackId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userPlaylistTrack);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserPlaylistTrackExists(userPlaylistTrack.PlaylistTrackId))
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
            ViewData["PlaylistId"] = new SelectList(_context.UserPlaylist, "PlaylistId", "PlaylistName", userPlaylistTrack.PlaylistId);
            ViewData["TrackId"] = new SelectList(_context.SongInfo, "TrackId", "Name", userPlaylistTrack.TrackId);
            return View(userPlaylistTrack);
        }

        // GET: UserPlaylistTracks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userPlaylistTrack = await _context.UserPlaylistTrack
                .Include(u => u.Playlist)
                .Include(u => u.Track)
                .FirstOrDefaultAsync(m => m.PlaylistTrackId == id);
            if (userPlaylistTrack == null)
            {
                return NotFound();
            }

            return View(userPlaylistTrack);
        }

        // POST: UserPlaylistTracks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userPlaylistTrack = await _context.UserPlaylistTrack.FindAsync(id);
            _context.UserPlaylistTrack.Remove(userPlaylistTrack);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserPlaylistTrackExists(int id)
        {
            return _context.UserPlaylistTrack.Any(e => e.PlaylistTrackId == id);
        }
    }
}
