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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Bardcore.ViewModels;

namespace Bardcore.Controllers
{
    public class UserPlaylistsController : Controller
    {
        private readonly BardcoreContext _context;
        private UserManager<IdentityUser> _userManager;
        private readonly IHostingEnvironment _webroot;

        public UserPlaylistsController(BardcoreContext context, IHostingEnvironment webroot, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _webroot = webroot;
        }

        //ViewModel to Display and Play Individual Tracks
        public IActionResult PlaylistIndividualTracks(int id)
        {
            PlaylistIndividualTracksVM playlistIndividualTracks = new PlaylistIndividualTracksVM();
            playlistIndividualTracks.userPlaylist = _context.UserPlaylist.FirstOrDefault(p => p.PlaylistCreator == id);

            var PlaylistQuery = _context.UserPlaylistTrack.Where(pl => pl.PlaylistId == id);

            var SongInfo = from c in PlaylistQuery
                           select c.TrackId;

            playlistIndividualTracks.songInfo = _context.SongInfo.Where(c => SongInfo.Contains(c.TrackId));

            return View(playlistIndividualTracks);
        }

        // GET: UserPlaylists
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            var bardcoreContext = _context.UserPlaylist.Include(u => u.PlaylistCreatorNavigation);
            return View(await bardcoreContext.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> Browse()
        {
            var bardcoreContext = _context.UserPlaylist.Include(u => u.PlaylistCreatorNavigation);
            return View(await bardcoreContext.ToListAsync());
        }

        // GET: UserPlaylists/Details/5
        [Authorize(Roles ="Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userPlaylist = await _context.UserPlaylist
                .Include(u => u.PlaylistCreatorNavigation)
                .FirstOrDefaultAsync(m => m.PlaylistId == id);
            if (userPlaylist == null)
            {
                return NotFound();
            }

            return View(userPlaylist);
        }

        // GET: UserPlaylists/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["PlaylistCreator"] = new SelectList(_context.UserProfile, "Userid", "DisplayName");
            return View();
        }

        // POST: UserPlaylists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlaylistId,PlaylistCreator,PlaylistName")] UserPlaylist userPlaylist)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userPlaylist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Browse));//original "(nameof(Index));"
            }
            ViewData["PlaylistCreator"] = new SelectList(_context.UserProfile, "Userid", "DisplayName", userPlaylist.PlaylistCreator);
            return View(userPlaylist);
        }


        // GET: UserPlaylists/Edit/5
        [Authorize(Roles ="Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userPlaylist = await _context.UserPlaylist.FindAsync(id);
            if (userPlaylist == null)
            {
                return NotFound();
            }
            ViewData["PlaylistCreator"] = new SelectList(_context.UserProfile, "Userid", "DisplayName", userPlaylist.PlaylistCreator);
            return View(userPlaylist);
        }

        // POST: UserPlaylists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles ="Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PlaylistId,PlaylistCreator,PlaylistName")] UserPlaylist userPlaylist)
        {
            if (id != userPlaylist.PlaylistId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userPlaylist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserPlaylistExists(userPlaylist.PlaylistId))
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
            ViewData["PlaylistCreator"] = new SelectList(_context.UserProfile, "Userid", "DisplayName", userPlaylist.PlaylistCreator);
            return View(userPlaylist);
        }

        // GET: UserPlaylists/Delete/5
        [Authorize(Roles ="Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userPlaylist = await _context.UserPlaylist
                .Include(u => u.PlaylistCreatorNavigation)
                .FirstOrDefaultAsync(m => m.PlaylistId == id);
            if (userPlaylist == null)
            {
                return NotFound();
            }

            return View(userPlaylist);
        }

        // POST: UserPlaylists/Delete/5
        [Authorize(Roles ="Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userPlaylist = await _context.UserPlaylist.FindAsync(id);
            _context.UserPlaylist.Remove(userPlaylist);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserPlaylistExists(int id)
        {
            return _context.UserPlaylist.Any(e => e.PlaylistId == id);
        }
    }
}
