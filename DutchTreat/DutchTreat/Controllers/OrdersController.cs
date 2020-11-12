using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Produces("application/json")]
    public class OrdersController : ControllerBase
    {
        private readonly IDutchRepository _repository;
        private readonly ILogger<OrdersController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<StoreUser> _userManager;

        public OrdersController(IDutchRepository repository,
            ILogger<OrdersController> logger,
            IMapper mapper,
            UserManager<StoreUser> userManager)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<IEnumerable<OrderViewModel>> Get(bool includeItems = true)
        {
            try
            {
                var username = User.Identity.Name;

                var results = _repository.GetAllOrdersByUser(username, includeItems);

                return Ok(_mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(results));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get orders: {ex}");

                return BadRequest("Failed to get orders");
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult<Order> Get(int id)
        {
            try
            {
                var order = _repository.GetOrderById(User.Identity.Name, id);

                if (order != null)
                {
                    return Ok(_mapper.Map<Order, OrderViewModel>(order)); 
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get order with ID {id}: {ex}");

                return BadRequest($"Failed to get order with ID {id}");
            }
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Order>> Post([FromBody]OrderViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newOrder = _mapper.Map<OrderViewModel, Order>(model);

                    if(newOrder.OrderDate == DateTime.MinValue)
                    {
                        newOrder.OrderDate = DateTime.Now;
                    }

                    var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

                    newOrder.User = currentUser;

                    _repository.AddEntity(newOrder);

                    if (_repository.SaveChanges())
                    {
                        return Created($"/api/orders/{newOrder.Id}", _mapper.Map<Order, OrderViewModel>(newOrder));
                    } 
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save a new order: {ex}");
            }

            return BadRequest("Failed to save a new order");
        }
    }
}
