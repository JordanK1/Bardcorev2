using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Bardcore.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Bardcore.Controllers
{
    public class UserProfilesController : Controller
    {
        private readonly BardcoreContext _context;
        private UserManager<IdentityUser> _userManager;
        private IHostingEnvironment _webroot;

        public UserProfilesController(BardcoreContext context, UserManager<IdentityUser> userManager, IHostingEnvironment webroot)
        {
            _context = context;
            _webroot = webroot;
            _userManager = userManager;
        }

        // GET: UserProfiles
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserProfile.ToListAsync());
        }

        // GET: UserProfiles/Details/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userProfile = await _context.UserProfile
                .FirstOrDefaultAsync(m => m.Userid == id);
            if (userProfile == null)
            {
                return NotFound();
            }

            return View(userProfile);
        }

        // GET: UserProfiles/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        public IActionResult ProfileInfo()
        {
            string userID = _userManager.GetUserId(User);
            UserProfile profile = _context.UserProfile.FirstOrDefault(p => p.UserAccountId == userID);

            if (profile == null)
            {
                return RedirectToAction("Create");

            }

            return View(profile);
        }

        // POST: UserProfiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Userid,FirstName,LastName,DisplayName,Gender,Age,Bio,UserAccountId,PhotoPath")] UserProfile userProfile,
            IFormFile FilePhoto)
        {
            if (FilePhoto != null)
            {

                string photoPath = _webroot.WebRootPath + "\\userPhotos\\";
                var fileName = Path.GetFileName(FilePhoto.FileName);

                using (var stream = System.IO.File.Create(photoPath + fileName))
                {
                    await FilePhoto.CopyToAsync(stream);
                    userProfile.PhotoPath = fileName;
                }
            }

            
            if (ModelState.IsValid)
            {
                _context.Add(userProfile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ProfileInfo));
            }
            return View(userProfile);
        }

        // GET: UserProfiles/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userProfile = await _context.UserProfile.FindAsync(id);
            if (userProfile == null)
            {
                return NotFound();
            }
            return View(userProfile);
        }

        // POST: UserProfiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Userid,FirstName,LastName,DisplayName,Gender,Age,Bio,UserAccountId,PhotoPath")] UserProfile userProfile)
        {
            if (id != userProfile.Userid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userProfile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserProfileExists(userProfile.Userid))
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
            return View(userProfile);
        }

        // GET: UserProfiles/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userProfile = await _context.UserProfile
                .FirstOrDefaultAsync(m => m.Userid == id);
            if (userProfile == null)
            {
                return NotFound();
            }

            return View(userProfile);
        }

        // POST: UserProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles ="Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userProfile = await _context.UserProfile.FindAsync(id);
            _context.UserProfile.Remove(userProfile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserProfileExists(int id)
        {
            return _context.UserProfile.Any(e => e.Userid == id);
        }
    }
}
