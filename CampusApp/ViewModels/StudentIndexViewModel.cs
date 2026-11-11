using CampusApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CampusApp.ViewModels
{
    public class StudentIndexViewModel
    {
        public IEnumerable<Student> Students { get; set; }

        public string Search { get; set; }

        public string Campus { get; set; }

        public IEnumerable<CampusWithCount> CampusesWithCount { get; set; }

        public IEnumerable<SelectListItem> CampusFilterItems
        {
            get
            {
                return CampusesWithCount.Select(cc => new SelectListItem
                {
                    Value = cc.CampusName,
                    Text = cc.NameWithCount
                });
            }
        }
    }

    public class CampusWithCount
    {
        public int StudentCount { get; set; }

        public string CampusName { get; set; }

        public string NameWithCount
        {
            get { return CampusName + " (" + StudentCount + ")"; }
        }
    }
}
