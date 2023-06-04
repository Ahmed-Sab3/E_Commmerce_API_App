using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;

namespace Talabat.APIs.Controllers
{
    public class BasketsController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketsController(IBasketRepository basketRepository,IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        [HttpGet]       //Get : api/baskets/id

        public async Task<ActionResult<CustomerBasket>> GetBasket(string Id) 
        {
                var basket = await _basketRepository.GetBasketAsync(Id);
            return basket ?? new CustomerBasket(Id);
        
        }
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket) 
        {
            var mappedBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);

            var createdOrUpdatedbasket = await _basketRepository.UpdateBasketAsync(mappedBasket);

            if (createdOrUpdatedbasket is null) return BadRequest(new ApiResponse(400));

            return createdOrUpdatedbasket;
        }

        [HttpDelete] //Delete : api/baskets
        public async Task<ActionResult<bool>> DeleteBasket(string Id) 
        {
            return await _basketRepository.DeletBasketAsync(Id);
        }



    }
}
