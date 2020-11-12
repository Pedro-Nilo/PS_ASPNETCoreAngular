using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext _context;
        private readonly IWebHostEnvironment _hosting;
        private readonly UserManager<StoreUser> _userManager;

        public DutchSeeder(DutchContext context, IWebHostEnvironment hosting, UserManager<StoreUser> userManager)
        {
            _context = context;
            _hosting = hosting;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            _context.Database.EnsureCreated();

            StoreUser user = await _userManager.FindByEmailAsync("pedroniloz.nicola@gmail.com");

            if(user == null)
            {
                user = new StoreUser()
                {
                    FirstName = "Pedro Nilo",
                    LastName = "Zonderico Nicola",
                    Email = "pedroniloz.nicola@gmail.com",
                    UserName = "pedroniloz.nicola@gmail.com"
                };

                var result = await _userManager.CreateAsync(user, "Novembro49!");

                if(result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create new user in seeder");
                }
            }

            if(!_context.Products.Any())
            {
                var filePath = Path.Combine(_hosting.ContentRootPath, "Data/art.json");
                var jsonData = File.ReadAllText(filePath);
                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(jsonData);

                _context.Products.AddRange(products);

                var order = _context.Orders.Where(order => order.Id == 1).FirstOrDefault();

                if(order != null)
                {
                    order.User = user;
                    order.Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Product = products.First(),
                            Quantity = 5,
                            UnitPrice = products.First().Price
                        }
                    };
                }

                _context.SaveChanges();
            }
        }
    }
}
