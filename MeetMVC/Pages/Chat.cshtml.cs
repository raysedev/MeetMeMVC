using MeetMVC.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MeetMVC.Pages
{
    public class ChatModel : PageModel
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        [ActivatorUtilitiesConstructorAttribute]
        public ChatModel(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public string UserId { get; set; }

        public class InputModel
        { 
        }

        private readonly ILogger<IndexModel> _logger;

        public ChatModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
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

                

            }

            return Page();
        }

        public async Task LoadAsync(ApplicationUser user)
        {


            Input = new InputModel
            {
                
            };
        }
    }
}
