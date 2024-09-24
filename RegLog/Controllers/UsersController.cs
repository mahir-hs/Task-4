using Microsoft.AspNetCore.Mvc;
using RegLog.Model;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace RegLog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UsersController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Index Page
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        // POST: ManageUsers Action
        [HttpPost]
        public async Task<IActionResult> ManageUsers(string actionType, string[] selectedUsers)
        {
            if (selectedUsers == null || selectedUsers.Length == 0)
            {
                TempData["StatusMessage"] = "No users selected!";
                return RedirectToAction(nameof(Index));
            }

            // Log selected users for debugging
            System.Diagnostics.Debug.WriteLine("Selected Users: " + string.Join(", ", selectedUsers));
            bool isCurrentUser = false;
            var originUser = await _userManager.GetUserAsync(User);
            foreach (var userId in selectedUsers)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    if (user.Id == originUser.Id && (actionType == "block" || actionType == "delete"))
                    {
                        isCurrentUser = true; // Set flag to indicate current user is blocked
                    }
                    if (actionType == "block")
                    {
                        user.Status = UserStatus.Blocked;
                    }
                    else if (actionType == "unblock")
                    {
                        user.Status = UserStatus.Active;
                    }
                    else if (actionType == "delete")
                    {
                        await _userManager.DeleteAsync(user);
                        continue;  // Skip update for deleted users
                    }

                    await _userManager.UpdateAsync(user);
                }
            }
            if (isCurrentUser)
            {
                // Log out the current user and redirect to the login page
                await _signInManager.SignOutAsync();
                return Redirect("/Identity/Account/Login");

            }
            return RedirectToAction(nameof(Index));
        }
    }
}
