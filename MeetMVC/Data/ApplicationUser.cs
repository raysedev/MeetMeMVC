using Microsoft.AspNetCore.Identity;

namespace MeetMVC.Data
{
    public class ApplicationUser : IdentityUser
    {

        public ApplicationUser() {
            Interests = new List<Interest>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Age { get; set; }
        public string About { get; set; }
        public string LookingFor { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Gender { get; set; }
        public string Sexuality { get; set; }
        public string ImagePath { get; set; }

        public virtual List<Interest> Interests { get; set; }
    }
}
