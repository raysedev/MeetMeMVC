using System.ComponentModel.DataAnnotations.Schema;

namespace MeetMVC.Data
{
    public class Interest
    {
        public Interest() { }

        public Interest(string name)
        {
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string ApplicationUserId { get; set; }
    }
}
