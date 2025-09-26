using chatmvc.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using chatmvc.Models;

namespace MyChat.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;

        public AdminController(UserManager<ApplicationUser> userManager, ApplicationDbContext db, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _db = db;
            _env = env;
        }

        // GET: /Admin/Index
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        // GET: /Admin/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var vm = new EditProfileViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                AvatarPath = user.AvatarPath
            };
            return View(vm);
        }

        // POST: /Admin/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProfileViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return NotFound();

            var other = await _userManager.FindByEmailAsync(model.Email);
            if (other != null && other.Id != user.Id)
            {
                ModelState.AddModelError(nameof(model.Email), "Email уже используется.");
                return View(model);
            }

            if (model.Avatar != null && model.Avatar.Length > 0)
            {
                var uploads = Path.Combine(_env.WebRootPath, "avatars");
                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);
                var fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(model.Avatar.FileName);
                var filePath = Path.Combine(uploads, fileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await model.Avatar.CopyToAsync(stream);
                user.AvatarPath = "/avatars/" + fileName;
            }

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.DateOfBirth = model.DateOfBirth;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var e in result.Errors) ModelState.AddModelError(string.Empty, e.Description);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: /Admin/Block/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Block(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            return RedirectToAction(nameof(Index));
        }

        // POST: /Admin/Unblock/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unblock(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            await _userManager.SetLockoutEndDateAsync(user, null);
            await _userManager.ResetAccessFailedCountAsync(user);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/Create
        public IActionResult Create() => View(new AdminCreateUserViewModel());

        // POST: /Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminCreateUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var today = DateTime.Today;
            var age = today.Year - model.DateOfBirth.Year;
            if (model.DateOfBirth.Date > today.AddYears(-age)) age--;
            if (age < 18)
            {
                ModelState.AddModelError(nameof(model.DateOfBirth), "Пользователь должен быть не моложе 18 лет.");
                return View(model);
            }

            string avatarPath = null;
            if (model.Avatar != null && model.Avatar.Length > 0)
            {
                var uploads = Path.Combine(_env.WebRootPath, "avatars");
                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);
                var fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(model.Avatar.FileName);
                var filePath = Path.Combine(uploads, fileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await model.Avatar.CopyToAsync(stream);
                avatarPath = "/avatars/" + fileName;
            }

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                DateOfBirth = model.DateOfBirth,
                AvatarPath = avatarPath
            };

            var create = await _userManager.CreateAsync(user, model.Password);
            if (!create.Succeeded)
            {
                foreach (var e in create.Errors) ModelState.AddModelError(string.Empty, e.Description);
                return View(model);
            }

            await _userManager.AddToRoleAsync(user, model.Role ?? "user");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            //var msg = await _db.ChatMessages.FindAsync(id);
            //if (msg == null) return NotFound();
            //_db.ChatMessages.Remove(msg);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
