using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.Entity;
using MeetMVC.Data;

namespace MeetMVC.Areas.Identity.Pages.Account.Manage
{
    public class InterestsModel : PageModel
    {
        public string Message { get; set; }
        public List<SelectListItem> Interests { get; set; }
        private ApplicationDbContext Context { get; }

        public InterestsModel(ApplicationDbContext _context)
        {
            this.Context = _context;
        }

        public void OnGet()
        {
            var interests = (from interest in this.Context.Interests
                          select new SelectListItem
                          {
                              Text = interest.Name,
                              Value = interest.Id.ToString()
                          }).ToList();
            this.Interests = interests;
        }

        public void OnPostSubmit()
        {
            var interests = (from interest in this.Context.Interests
                             select new SelectListItem
                             {
                                 Text = interest.Name,
                                 Value = interest.Id.ToString()
                             }).ToList();
            this.Interests = interests;

            string[] interestIds = Request.Form["lstInterests"].ToString().Split(",");
            foreach (string id in interestIds)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    string name = this.Interests.Where(x => x.Value == id).FirstOrDefault().Text;
                    this.Message += "Id: " + id + "  Interest: " + name + "\\n";
                }
            }
        }
    }
}
