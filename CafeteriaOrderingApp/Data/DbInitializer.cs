using CafeteriaOrderingApp.Models;
using CafeteriaOrderingApp.Services;

namespace CafeteriaOrderingApp.Data
{
    public class DbInitializer
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            context.Database.EnsureCreated();

            // Skip if data already exists
            if (context.Employees.Any()) return;

            // Seed Employees
            var employees = new List<Employee>
        {
            new Employee { Name = "Sipho Dlamini", EmployeeNumber = "EMP001", Balance = 0, LastDepositMonth = DateTime.Now },
            new Employee { Name = "Thandi Nkosi", EmployeeNumber = "EMP002", Balance = 0, LastDepositMonth = DateTime.Now },
            new Employee { Name = "Lebo Mokoena", EmployeeNumber = "EMP003", Balance = 0, LastDepositMonth = DateTime.Now },
            new Employee { Name = "Ayanda Sithole", EmployeeNumber = "EMP004", Balance = 0, LastDepositMonth = DateTime.Now },
            new Employee { Name = "Kgosi Moeketsi", EmployeeNumber = "EMP005", Balance = 0, LastDepositMonth = DateTime.Now }
        };
            context.Employees.AddRange(employees);

            // Seed Restaurants and Menu Items
            var mcd = new Restaurant
            {
                Name = "McDonald's Braamfontein",
                LocationDescription = "Jan Smuts Avenue",
                ContactNumber = "011 123 4567",
                MenuItems = new List<MenuItem>
            {
                new MenuItem { Name = "Big Mac Meal", Description = "Burger, fries & drink", Price = 75 },
                new MenuItem { Name = "McChicken Meal", Description = "Chicken burger combo", Price = 65 },
                new MenuItem { Name = "Boerie & Hash Brown Stack", Description = "Regular Burger", Price = 35 },
                new MenuItem { Name = "Cappuccino", Description = "Hot Drink", Price = 20 },
                new MenuItem { Name = "Strawberry Shake", Description = "Milkshake", Price = 40 }
            }
            };

            var kfc = new Restaurant
            {
                Name = "KFC Braamfontein",
                LocationDescription = "De Korte Street",
                ContactNumber = "011 234 5678",
                MenuItems = new List<MenuItem>
            {
                new MenuItem { Name = "Streetwise Two", Description = "2 pcs chicken & fries", Price = 50 },
                new MenuItem { Name = "Zinger Wings", Description = "Spicy wings", Price = 40 },
                new MenuItem { Name = "Sweet Chilli Crunch Master", Description = "Wrap", Price = 45 },
                new MenuItem { Name = "Mega Wing Box Buddy", Description = "Box Meal", Price = 60 },
                new MenuItem { Name = "Dunked Wings", Description = "Snack", Price = 50 }
            }
            };

            context.Restaurants.AddRange(mcd, kfc);
            context.SaveChanges();
        }
    }

}

