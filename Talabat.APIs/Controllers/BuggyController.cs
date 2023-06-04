using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Repository.Data;

namespace Talabat.APIs.Controllers
{
  
    public class BuggyController : BaseApiController
    {
        private readonly StoreContext _dbcontext;

        public BuggyController(StoreContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet("notfound")]//Get/Api/Buggy/notfound
        public ActionResult GetNotFoundRequest() 
        {
            var product = _dbcontext.Products.Find(100);
            if (product is null) return NotFound(new ApiResponse(404));
            return Ok(product);
        }

        [HttpGet("servererror")]//Get/Api/Buggy/servererror
        public ActionResult GetServerError() 
        {
            var product = _dbcontext.productBrands.Find(100);
             var productToReturn = product.ToString();//will Throw Exception [Null ReferenceException]

            return Ok(productToReturn);
        
        }
        [HttpGet("badrequest")]//Get/Api/Buggy/badrequest


        public ActionResult GetBadRequest() 
        {
            return BadRequest(new ApiResponse(400));
        
        }

        [HttpGet("badrequest/{id}")]//Get/Api/Buggy/badrequest/five

        public ActionResult GetBadRequest(int id) //validationError
        {
            return Ok();
        }

    }
}
