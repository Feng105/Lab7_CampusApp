# Campus App — ASP.NET MVC 5 with Entity Framework (Lab 7)

A basic ASP.NET MVC website built with Entity Framework Code First for **Lab 7 / Topic 8** of the Web Development course, extended in **Lab 8** with student search, campus filtering and a strongly typed view model. It contains two parts: the tutorial project (Categories and Products) and the lab task (University Campuses and Students in a one-to-many relationship).

## Requirements

- Visual Studio 2022 with the **ASP.NET and web development** workload
- .NET Framework 4.7.2
- SQL Server LocalDB (`(localdb)\MSSQLLocalDB`), installed with Visual Studio
- NuGet packages are restored automatically from `packages.config` on the first build

## Running the site

1. Open `CampusApp.sln` in Visual Studio 2022.
2. Press `Ctrl+F5` (Debug → Start Without Debugging).
3. The database `CampusContext-20261110160000.mdf` is created in `App_Data` the first time a page reads data.

## What is inside

| Area | Contents |
| --- | --- |
| `Models` | `Product`, `Category` (tutorial), `Student`, `UniversityCampus` (lab) |
| `Data` | `CampusContext` — one `DbContext` with four `DbSet`s and its connection string in `Web.config` |
| `Controllers` | `HomeController`, `ProductsController`, `CategoriesController`, `StudentsController`, `UniversityCampusesController` |
| `Views` | Scaffolded CRUD pages for Products, Categories, Students and Campuses |

## The one-to-many relationship

```csharp
public class Student
{
    [Key] public int ID { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }

    public int? CampusID { get; set; }                     // foreign key, nullable
    public virtual UniversityCampus Campus { get; set; }    // reference navigation property
}

public class UniversityCampus
{
    [Key] public int ID { get; set; }
    public string Name { get; set; }

    public virtual ICollection<Student> Students { get; set; }  // collection navigation property
}
```

The foreign key `CampusID` sits on the `Student` table, so many students can point at the same campus while each student points at most one campus. Making it `int?` keeps the relationship optional: a student may not belong to a campus at all. Entity Framework Code First creates the column and the foreign key constraint by convention (the `<navigation property>ID` naming pattern), so no Fluent API configuration is needed.

The navigation properties let the application walk the relationship through objects instead of joins: `student.Campus.Name` returns the campus of a student, `campus.Students` enumerates the students of a campus, `db.Students.Include(s => s.Campus)` eager-loads the related campus for a list page, and the MVC scaffolding uses the collection to render the campus drop-down on the Student create and edit forms.

## Lab 8 — searching, filtering and view models

`StudentsController.Index` now takes two query-string parameters and does three things in one request:

```csharp
public ActionResult Index(string campus, string search)
{
    StudentIndexViewModel viewModel = new StudentIndexViewModel();
    var students = db.Students.Include(s => s.Campus);

    if (!String.IsNullOrEmpty(search))
    {
        students = students.Where(s => s.Name.Contains(search) ||
                                       s.Address.Contains(search) ||
                                       s.Campus.Name.Contains(search));
        viewModel.Search = search;
    }

    viewModel.CampusesWithCount = from s in students
                                  where s.CampusID != null
                                  group s by s.Campus.Name into campusGroup
                                  select new CampusWithCount
                                  {
                                      CampusName = campusGroup.Key,
                                      StudentCount = campusGroup.Count()
                                  };

    if (!String.IsNullOrEmpty(campus))
    {
        students = students.Where(s => s.Campus.Name == campus);
    }

    viewModel.Students = students.OrderBy(s => s.Campus.Name).ThenBy(s => s.Name).ToList();
    viewModel.Campus = campus;

    return View(viewModel);
}
```

- **Search** is a LINQ `Contains` over the student name, the address and the related campus name. The navigation property is available because `Include(s => s.Campus)` eager-loads it.
- **Campus filter** is a second `Where` on the same query, so search and filter combine.
- **Ordering matters**: the campus list with its counts is built *before* the campus filter is applied. Building it afterwards would leave only the selected campus in the drop-down (and a count of zero for everything else), which is the trap the tutorial warns about.
- **View model instead of ViewBag**: `StudentIndexViewModel` carries `Students`, `Search`, `Campus`, `CampusesWithCount` and the derived `CampusFilterItems`, so the strongly typed view reads `Model.Students` and `Model.CampusFilterItems` instead of `ViewBag`. `CampusWithCount.NameWithCount` produces the `Manawatu Campus (2)` text shown in the drop-down.
- **Search box in the navigation bar** (`Views/Shared/_Layout.cshtml`) is a GET form posting `Search` to `Students/Index`, so the term travels in the URL and survives the campus filter through the hidden field in the filter form.

The intermediate ViewBag version was exercised first (search plus `ViewBag.Campus` as a `SelectList`); `docs/screenshots/lab8/01`–`04` were captured while that version was live, and `05`–`08` show the final view-model version with the campus counts.

## Notes

- `packages/`, `bin/`, `obj/` and the generated `.mdf` database are not committed; NuGet restore and the first run recreate them.
- The connection string lives in the root `Web.config`, under the name `CampusContext`, which is the name passed to `base(...)` in `CampusContext`.
