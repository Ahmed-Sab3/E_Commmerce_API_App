using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specicfications;

namespace Talabat.APIs.Controllers
{

    public class ProductsController : BaseApiController
    {
        private readonly IUntiOfWork _untiOfWork;

        ///private readonly IGenericRepository<ProductBrand> _brandsRepo;
        ///private readonly IGenericRepository<ProductType> _typesRepo;
        ///private readonly IGenericRepository<Product> _productsRepo;

        private readonly IMapper _mapper;

        public ProductsController(
            IUntiOfWork untiOfWork
            , IMapper mapper)
        {
            _untiOfWork = untiOfWork;
            ///_brandsRepo = brandsRepo;
            ///_typesRepo = typesRepo;
            ///_productsRepo = ProductsRepo;
            _mapper = mapper;
        }


        //[Authorize] 
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProduct([FromQuery]ProductSpecParams specParams)
         {
            var spec = new ProductWithPrandAndTypeSpecicfication(specParams);

            var products = await _untiOfWork.Repository<Product>().GetAllwithSpecAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            var countspec = new ProductwithFilterationForCountSpecification(specParams);
            var count = await _untiOfWork.Repository<Product>().GetCountWithSpecAsync(countspec);

             return Ok(new Pagination<ProductToReturnDto>(specParams.PageIndex,specParams.PageSize, count, data));
         }

        [ProducesResponseType(typeof(ProductToReturnDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> Getproduct(int id) 
        {
            var spec = new ProductWithPrandAndTypeSpecicfication(id);

            var product = await _untiOfWork.Repository<Product>().GetByIdWithSpecAsync(spec);
            if (product is null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<Product,ProductToReturnDto>(product));
        
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands() 
        {
            var brands = await _untiOfWork.Repository<ProductBrand>().GetAllAsync();
            return Ok(brands);
        }
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var types = await _untiOfWork.Repository<ProductType>().GetAllAsync();
            return Ok(types);
        }

    }
}
