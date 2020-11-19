using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DutchTreat.Controllers
{
    [Route("api/orders/{orderId:int}/items")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Produces("application/json")]
    public class OrderItemsController : ControllerBase
    {
        private readonly IDutchRepository _repository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public OrderItemsController(IDutchRepository repository, ILogger<OrderItemsController> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<OrderItem>> Get(int orderId)
        {
            try
            {
                var order = _repository.GetOrderById(User.Identity.Name, orderId);

                if (order != null)
                {
                    return Ok(_mapper.Map<IEnumerable<OrderItem>, IEnumerable<OrderItemViewModel>>(order.Items));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get items from order with ID {orderId}: {ex}");

                return BadRequest($"Failed to get items from order with ID {orderId}");
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult<OrderItem> Get(int orderId, int id)
        {
            try
            {
                var order = _repository.GetOrderById(User.Identity.Name, orderId);

                if (order != null)
                {
                    var item = order.Items.Where(item => item.Id == id).FirstOrDefault();

                    if (item != null)
                    {
                        return Ok(_mapper.Map<OrderItem, OrderItemViewModel>(item)); 
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get item with ID {id} from order with ID {orderId}: {ex}");

                return BadRequest($"Failed to get item with ID {id} from order with ID {orderId}");
            }
        }
    }
}
