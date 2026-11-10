using System.ComponentModel.DataAnnotations;

namespace CampusApp.Models
{
    public class Student
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public int? CampusID { get; set; }

        public virtual UniversityCampus Campus { get; set; }
    }
}
