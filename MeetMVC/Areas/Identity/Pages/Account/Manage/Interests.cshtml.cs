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
        public List<SelectListItem> UserInterests { get; set; }
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

        private List<SelectListItem> GetAllInterestsAsListItems()
        {
            return (from interest in interestsLst
                    select new SelectListItem
                    {
                        Text = interest,
                        Value = interest
                    }).ToList();
        }

        private List<SelectListItem> GetUserInterestsAsListItems(string userId)
        {
            return (from interesetName in GetUserInterestsNames(userId)
                    select new SelectListItem
                    {
                        Text = interesetName,
                        Value = interesetName
                    }).ToList();
        }

        private List<String> GetUserInterestsNames(string userId)
        {
            return this.Context.Interests.Where(x => x.ApplicationUserId == userId).Select(i => i.Name).ToList();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            this.Interests = GetAllInterestsAsListItems();

            var user = await _userManager.GetUserAsync(User);
            this.UserInterests = GetUserInterestsAsListItems(user.Id);

            return Page();
        }

        public async Task<IActionResult> OnPostSubmitAsync()
        {
            this.Interests = GetAllInterestsAsListItems();

            var user = await _userManager.GetUserAsync(User);

            var namesOfCurrentInterests = GetUserInterestsNames(user.Id);

            var namesOfSelectedInterests = Request.Form["lstInterests"].ToString().Split(",").ToHashSet();

            foreach (var name in namesOfSelectedInterests)
            {
                if (!namesOfCurrentInterests.Contains(name))
                {
                    user.Interests.Add(new Interest
                    {
                        Name = name
                    });
                }
            }
            await _userManager.UpdateAsync(user);

            this.UserInterests = GetUserInterestsAsListItems(user.Id);
            return RedirectToPage();
        }
    }
}