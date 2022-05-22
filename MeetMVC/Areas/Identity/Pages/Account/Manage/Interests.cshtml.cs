using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.Entity;
using MeetMVC.Data;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MeetMVC.Areas.Identity.Pages.Account.Manage
{
    public class InterestsModel : PageModel
    {
        public string Message { get; set; }
        public List<SelectListItem> Interests { get; set; }
        private ApplicationDbContext Context { get; }
        //todo readonly
        private static readonly string[] interestsLst = { "sports", "gamings", "cooking" };

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public InterestsModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext _context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.Context = _context;
        }
        public string Image { get; set; }
        public string Username { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public List<string> UserInterests { get; set; }
        }

        private List<SelectListItem> GetInterests()
        {
            return (from interest in interestsLst
                             select new SelectListItem
                             {
                                 Text = interest,
                                 Value = interest
                             }).ToList();
            //this.Interests = interests;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            this.Interests = GetInterests();
            var user = await _userManager.GetUserAsync(User);

            List<string> interestNames = new List<string>();
            /*foreach (var interest in user.Interests)
            {
                interestNames.Add(interest.Name);
            }*/

            var interestList = this.Context.Interests.Where(x => x.ApplicationUserId == user.Id).ToList();

            foreach (var interest in interestList)
            {
                interestNames.Add(interest.Name);
            }

            Input = new InputModel
            {
                UserInterests = interestNames
            };

            this.Message = interestNames.Count().ToString() + " " + user.Interests.Count();

            return Page();
        }

        public async Task<IActionResult> OnPostSubmitAsync()
        {

            this.Interests = GetInterests();
            var user = await _userManager.GetUserAsync(User);

            string[] interestNames = Request.Form["lstInterests"].ToString().Split(",");
            foreach (string name in interestNames)
            {

                user.Interests.Add(new Interest
                {
                    Name = name
                });
    
                this.Message += "Interest: " + name + "\\n";
            }

            await _userManager.UpdateAsync(user);
            return RedirectToPage();
        }
    }
}
