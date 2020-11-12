using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DutchTreat.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext _context;
        private readonly ILogger<DutchRepository> _logger;

        public DutchRepository(DutchContext context, ILogger<DutchRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            try
            {
                return _context.Products
                               .OrderBy(product => product.Title)
                               .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all products {ex}");

                return null;
            }
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            try
            {
                return _context.Products
                               .Where(product => product.Category == category)
                               .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get products from {category} category {ex}");

                return null;
            }
        }

        public IEnumerable<Order> GetAllOrders(bool includeItems)
        {
            try
            {
                if (includeItems)
                {
                    return _context.Orders
                           .Include(order => order.Items)
                           .ThenInclude(item => item.Product)
                           .ToList(); 
                }
                else
                {
                    return _context.Orders
                           .ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all orders {ex}");

                return null;
            }
        }

        public IEnumerable<Order> GetAllOrdersByUser(string username, bool includeItems)
        {
            try
            {
                if (includeItems)
                {
                    return _context.Orders
                           .Where(order => order.User.UserName == username)
                           .Include(order => order.Items)
                           .ThenInclude(item => item.Product)
                           .ToList();
                }
                else
                {
                    return _context.Orders
                           .Where(order => order.User.UserName == username)
                           .ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all orders {ex}");

                return null;
            }
        }

        public Order GetOrderById(string username, int id)
        {
            try
            {
                return _context.Orders
                               .Include(order => order.Items)
                               .ThenInclude(item => item.Product)
                               .Where(order => order.Id == id && order.User.UserName == username)
                               .FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get order with ID {id} {ex}");

                return null;
            }
        }

        public void AddEntity(object model)
        {
            try
            {
                _context.Add(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save new entity {ex}");
            }
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
