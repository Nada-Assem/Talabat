using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using Talabat.APIS.DTOS;
using Talabat.APIS.Errors;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;

namespace Talabat.APIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthService _authService;

        public AccountController(UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager,
            IAuthService authService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
        }


        //POST: /api/account/login
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO model)
        {
            var user =  await _userManager.FindByEmailAsync(model.Email);   

            if (user == null)
            return Unauthorized(new ApiResponse(401));
           

            var result = await _signInManager.CheckPasswordSignInAsync(user , model.Password , false);
            if (result.Succeeded is false)
                return Unauthorized(new ApiResponse(401));

             return Ok(new UserDTO()
            {
                Email = user.Email ,
                DisplayName = user.DisplayName ,    
                Token = await _authService.CreateTokenAsync(user , _userManager)
            });
        }

        

        //POST: /api/account/register
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO model)
        {
            var user = new AppUser(){
                DisplayName = model.DisplayName ,
                Email = model.Email ,
                PhoneNumber = model.PhoneNumber ,
                UserName = model.Email.Split('@')[0],
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded is false)
                return BadRequest(new ApiResponse(400));

            return Ok(new UserDTO()
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });
        }
    }
}
