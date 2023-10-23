using BookListing.Domain.Entities;
using BookListing.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using System.Drawing;

namespace BookListing.Infrastructure.Data
{
    public static class ApplicationDBContextSeed
    {
        public static async Task SeedDefaultUserAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            var roles = new[] { "Admin", "User" };
            foreach (var role in roles)
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));


            {
                //ADMIN ACCOUNT
                string email = "admin@localhost";
                string password = "Password1!";
                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new User();
                    user.UserName = email;
                    user.Email = email;
                    await userManager.CreateAsync(user, password);
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }

            {
                //SIMPLE USER ACCOUNT
                string email = "bernie@localhost";
                string password = "Password1!";
                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new User();
                    user.UserName = email;
                    user.Email = email;
                    await userManager.CreateAsync(user, password);
                    await userManager.AddToRoleAsync(user, "User");
                }
            }
        }

        public static async Task SeedSampleDataAsync(UserManager<User> userManager, ApplicationDbContext context)
        {
            if (!context.Categories.Any())
            {
                var categories = new Category[]
                {
                    new Category { Name = "Science" },
                    new Category { Name = "Arts" },
                    new Category { Name = "History" },
                    new Category { Name = "Mathematics" }, 
                    new Category { Name = "Philosophy" }   
                };

                foreach (var category in categories)
                {
                    context.Categories.Add(category);
                }
                await context.SaveChangesAsync();
            }

            if (!context.Departments.Any())
            {
                var departments = new Department[]
                {
                    new Department { Name = "Education" },
                    new Department { Name = "Research" },
                    new Department { Name = "Engineering" },
                    new Department { Name = "Humanities" },
                    new Department { Name = "Medicine" }
                };

                foreach (var department in departments)
                {
                    context.Departments.Add(department);
                }
                await context.SaveChangesAsync();
            }

            if (!context.Books.Any())
            {
                var researchDepartment = context.Departments.First(x => x.Name == "Research");
                var scienceCategory = context.Categories.First(x => x.Name == "Science");
                var artsCategory = context.Categories.First(x => x.Name == "Arts");
                var engineeringDepartment = context.Departments.First(x => x.Name == "Engineering");
                var mathematicsCategory = context.Categories.First(x => x.Name == "Mathematics");
                var humanitiesDepartment = context.Departments.First(x => x.Name == "Humanities");
                var historyCategory = context.Categories.First(x => x.Name == "History");
                var medicineDepartment = context.Departments.First(x => x.Name == "Medicine");

                var books = new Book[]
                {

                    new Book { Title = "Physics 101", Author = "Dr. Alan Smith", Description = "Introduction to Physics.", Department = researchDepartment },
                    new Book { Title = "Artistic Expressions", Author = "Ms. Beatrice Evans", Description = "Exploring Art Through History.", Category = artsCategory },
                    new Book { Title = "Data Structure and Algorithms", Author = "Bernie Ngojo", Description = "Saddest Book Ever.", Department = researchDepartment },

                    new Book { Title = "Engineering Basics", Author = "Prof. Charles O'Donnell", Description = "Fundamentals of Engineering.", Category = scienceCategory, Department = engineeringDepartment },
                    new Book { Title = "Calculus Advanced", Author = "Dr. Alan Smith", Description = "Dive into the depths of calculus.", Category = mathematicsCategory, Department = researchDepartment },
                    new Book { Title = "World Philosophies", Author = "Ms. Diana Hayes", Description = "A journey through philosophical ideas.", Category = artsCategory, Department = humanitiesDepartment },
                    new Book { Title = "Medicine Through Ages", Author = "Dr. Fiona Wright", Description = "Historical account of medical practices.", Category = historyCategory, Department = medicineDepartment },
                    new Book { Title = "Quantum Mechanics", Author = "Prof. Charles O'Donnell", Description = "Understanding the quantum world.", Category = scienceCategory, Department = researchDepartment },
                    new Book { Title = "Renaissance Art and Its Impact", Author = "Ms. Beatrice Evans", Description = "The influence of renaissance art.", Category = artsCategory, Department = humanitiesDepartment },
                    new Book { Title = "Digital Systems", Author = "Prof. Gary Hughes", Description = "Foundations of digital electronics.", Category = scienceCategory, Department = engineeringDepartment },
                    new Book { Title = "Algebra and its Applications", Author = "Dr. Ian Fletcher", Description = "Dive into algebraic structures.", Category = mathematicsCategory, Department = researchDepartment },
                    new Book { Title = "Evolution of Humanity", Author = "Ms. Diana Hayes", Description = "Tracing human origins and developments.", Category = historyCategory, Department = humanitiesDepartment },
                    new Book { Title = "Clinical Practices Today", Author = "Dr. Fiona Wright", Description = "Modern approaches to healthcare.", Category = scienceCategory, Department = medicineDepartment }
                };

                foreach (var book in books)
                {
                    context.Books.Add(book);
                }
                await context.SaveChangesAsync();
            }



            //Create user access seed
            if (!context.UserAccesses.Any())
            {
                //add default access to simple user
                var user = await userManager.FindByNameAsync("bernie@localhost");
                var categoryFound = context.Categories.First(x => x.Name == "Science");

                var userAccess1 = new UserAccess
                {
                    Category = categoryFound,
                    UserId = user.Id
                };
                context.UserAccesses.Add(userAccess1);

                var deptFound = context.Departments.First(x => x.Name == "Research");
                var userAccess2 = new UserAccess
                {
                    Department = deptFound,
                    UserId = user.Id
                };
                context.UserAccesses.Add(userAccess2);
                await context.SaveChangesAsync();
            }
        }

    }


}
