using Microsoft.AspNetCore.Mvc.Rendering;

namespace MeetMVC.Models
{
    public class InterestsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<SelectListItem> ItemList { get; set; }
    }

    public class PostSelectedViewModel
    {
        public int[] SelectedIds { get; set; }
    }
}
