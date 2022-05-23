using MeetMVC.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MeetMVC.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        [ActivatorUtilitiesConstructorAttribute]
        public IndexModel(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            Users = new List<ApplicationUser>();
        }

        [BindProperty]
        public InputModel Input { get; set; }

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

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public string InterestedGenders { get; set; }

        public async Task LoadAsync(ApplicationUser user)
        {
            if (user.Sexuality == "Gay")
            {
                InterestedGenders = user.Gender;
            }
            else if (user.Sexuality == "Straight" && user.Gender == "Man")
            {
                InterestedGenders = "Woman";
            }
            else if (user.Sexuality == "Straight" && user.Gender == "Woman")
            {
                InterestedGenders = "Man";
            }

            Users = await _userManager.Users
                .Where(item => InterestedGenders.Contains(item.Gender))
                .OrderBy(item => Guid.NewGuid())
                .Take(10)
                .ToListAsync();



            Input = new InputModel
            {
                Sexuality = user.Sexuality
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {

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