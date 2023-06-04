using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{
    public class AccountsController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenServic _tokenServic;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,ITokenServic tokenServic,IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenServic = tokenServic;
            _mapper = mapper;
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model) 
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user == null)  return Unauthorized(new ApiResponse(401)); 
            var result = await _signInManager.CheckPasswordSignInAsync(user,model.Password,false);
            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));
            return Ok(new UserDto(){
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token=await _tokenServic.CreateTokenAsync(user,_userManager)

            
            });;
        }


        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model) 
        {
            if (CheckEmailExist(model.Email).Result.Value)
                return BadRequest(new ApiValidationErrorResponse() {Errors = new string[] {"This Email Already In Use!!"} });

            var user = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                PhoneNumber= model.PhoneNumber,

            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));
            return Ok(new UserDto()
            {
                DisplayName=user.DisplayName,
                Email=user.Email,
                Token = await _tokenServic.CreateTokenAsync(user, _userManager)

            });
        }

        [Authorize]
        [HttpGet] //GET :/api/accouts
        public async Task<ActionResult<UserDto>> GetCurrentUser() 
         {
            var email =User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            return Ok(new UserDto()
            {
                DisplayName=user.DisplayName,
                Email=user.Email,
                Token = await _tokenServic.CreateTokenAsync(user,_userManager)

            });
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress() 
        {

            var user = await _userManager.FindUserWithAddressAsync(User);

            var mappedAddress = _mapper.Map<Address, AddressDto>(user.Address);
           
            return Ok(mappedAddress);   
        
        
        }


        [Authorize]
        [HttpPut("address")] // PUT :/api/accounts/address
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto updatedAddress) 
        {
            var address = _mapper.Map<AddressDto, Address>(updatedAddress);


            var user = await _userManager.FindUserWithAddressAsync(User);

            address.Id = user.Address.Id;


            user.Address = address;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(new ApiResponse(401));

            return Ok(updatedAddress);
        }


        [HttpGet("emailexists")] //GET :/api/accounts/emailexists
        public async Task<ActionResult<bool>> CheckEmailExist(string email) 
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        
        }



    }
}
