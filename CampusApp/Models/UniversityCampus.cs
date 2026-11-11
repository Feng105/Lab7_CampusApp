using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CampusApp.Models
{
    public class UniversityCampus
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Campus Name")]
        public string Name { get; set; }

        public virtual ICollection<Student> Students { get; set; }
    }
}
