using System.ComponentModel.DataAnnotations;

namespace CampusApp.Models
{
    public class Student
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Student Name")]
        public string Name { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        public int? CampusID { get; set; }

        public virtual UniversityCampus Campus { get; set; }
    }
}
