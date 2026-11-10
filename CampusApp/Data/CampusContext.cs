using System.Data.Entity;

namespace CampusApp.Data
{
    public class CampusContext : DbContext
    {
        public CampusContext() : base("name=CampusContext")
        {
        }

        public System.Data.Entity.DbSet<CampusApp.Models.Category> Categories { get; set; }

        public System.Data.Entity.DbSet<CampusApp.Models.Product> Products { get; set; }

        public System.Data.Entity.DbSet<CampusApp.Models.UniversityCampus> UniversityCampuses { get; set; }

        public System.Data.Entity.DbSet<CampusApp.Models.Student> Students { get; set; }
    }
}
