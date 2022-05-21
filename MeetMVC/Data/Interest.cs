using System.ComponentModel.DataAnnotations.Schema;

namespace MeetMVC.Data
{
    public class Interest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ApplicationUserId { get; set; }
    }
}
