using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext _context;
        private readonly IWebHostEnvironment _hosting;

        public DutchSeeder(DutchContext context, IWebHostEnvironment hosting)
        {
            _context = context;
            _hosting = hosting;
        }

        public void Seed()
        {
            _context.Database.EnsureCreated();

            if(!_context.Products.Any())
            {
                var filePath = Path.Combine(_hosting.ContentRootPath, "Data/art.json");
                var jsonData = File.ReadAllText(filePath);
                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(jsonData);

                _context.Products.AddRange(products);

                var order = _context.Orders.Where(order => order.Id == 1).FirstOrDefault();

                if(order != null)
                {
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
