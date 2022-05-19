using MeetMVC.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace MeetMVC.Pages
{
    public class UserModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        [ActivatorUtilitiesConstructorAttribute]
        public UserModel(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            Users = new List<ApplicationUser>();
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public string UserId { get; set; }
        public string ImagePath { get; set; }
        public string ReturnUrl { get; set; }
        public List<ApplicationUser> Users { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            public string Sexuality { get; set; }
        }

        private readonly ILogger<IndexModel> _logger;

        public UserModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task LoadAsync(ApplicationUser user)
        {
            string interestedGenders = user.LookingFor;
            Users = await _userManager.Users
                .Where(item => interestedGenders.Contains(item.Gender))
                .ToListAsync();



            Input = new InputModel
            {
                Sexuality = user.Sexuality
            };
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            UserId = id;
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                await LoadAsync(user);
            }
            else
            {

                Users = await _userManager.Users.OrderBy(item => Guid.NewGuid()).Take(10).ToListAsync();

            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            if (Input.Sexuality != "")
            {
                user.Sexuality = Input.Sexuality;
                await _userManager.UpdateAsync(user);
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
