using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CampusApp.Models
{
    public class Category
    {
        [Key]
        public int ID { get; set; }
        //By convention,
        //a property named Id or <type name>Id
        //will be configured as the primary
        //key of an entity.
        public string Name { get; set; }
        public virtual ICollection<Product> Products{get; set;}
    }
}