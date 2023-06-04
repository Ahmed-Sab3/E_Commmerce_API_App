using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.OrderAggregate;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService,IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [ProducesResponseType(typeof(Order),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
        [HttpPost] //Post ://api/orders
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto) 
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var shippingAddress = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);


            var order = await _orderService.CreateOrderAsync(buyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, shippingAddress);


            if (order is null) return BadRequest(new ApiResponse(400));
            return Ok(_mapper.Map<Order,OrderToReturnDto>(order));
        }

        [HttpGet]  //Get ://api/orders
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser() 
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _orderService.GetOrdersForUserAsync(buyerEmail);


            return Ok(_mapper.Map<IReadOnlyList<Order>,IReadOnlyList<OrderToReturnDto>>( orders));
        
        }


        [ProducesResponseType(typeof(OrderToReturnDto),StatusCodes.Status200OK)]
        [HttpGet("{id}")] //Get :/api/order/1
        public async Task<ActionResult<Order>> GetOrderForUser(int id) 
        {

            var email = User.FindFirstValue(ClaimTypes.Email);

            var order = await _orderService.GetOrderByIdForUserAsync(email,id);
            if (order is null) return NotFound(new ApiResponse(404));
            return Ok(_mapper.Map<Order,OrderToReturnDto>(order));

        }


        [HttpGet("deliverymethods")]  //Get "/api/orders/deliverymethods
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveyMethods() 
        {
            var deliveryMethods = await _orderService.GetDeliveryMethodAsync();
            return Ok(deliveryMethods);



        }



    }
}
