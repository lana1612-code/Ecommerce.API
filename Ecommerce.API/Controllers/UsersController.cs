using Ecommerce.Core.Entities;
using Ecommerce.Core.Entities.DTO;
using Ecommerce.Core.IRepositories;
using Ecommerce.Core.IRepositories.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepositories usersRepositories;
        private readonly UserManager<LocalUser> userManager;
        private readonly IEmailService emailService;

        public UsersController(IUsersRepositories usersRepositories
            ,UserManager<LocalUser> userManager
            ,IEmailService emailService)
        {
            this.usersRepositories = usersRepositories;
            this.userManager = userManager;
            this.emailService = emailService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            if(ModelState.IsValid)
            {
                var user = await usersRepositories.Login(model);
                if(user == null)
                {
                    return Unauthorized(new ApiValidationResponse(new List<string>()
                   { "Email and password inCorrect !!" }, 401));
                }
                return Ok(user);
            }
            return BadRequest(new ApiValidationResponse(new List<string>()
            { "Please try to enter the email and password correcty !!" },400));
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterationRequestDTO model)
        {
            try
            {
                var uniqueUser = usersRepositories.IsUniqueUser(model.Email);
                if( ! uniqueUser )
                {
                    return BadRequest(new ApiResponse(400, "the Email is already exist !!"));
                }

                var user = await usersRepositories.Register(model);
                if ( user == null )
                {
                      return BadRequest(new ApiResponse(400, "error while regestration user"));
                }
                else
                {
                    return Ok(new ApiResponse(201,result:user));
                }

            }catch (Exception ex)
            {
                return StatusCode(500, new ApiValidationResponse(new List<string>() { ex.Message,
                    "an error while posting your request !" }));
            }
        }

        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmailForUser([FromQuery]string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest(new ApiValidationResponse(
                    new List<string>  { $"This Email {email} not found :(" }));

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var ForgetPassword = Url.Action("ResetPassword" , "Users", new {token=token,
                                                email = user.Email}, Request.Scheme);
            var subject = "Reset password Request";
            var message = $"Please click on the link to reset your password : {ForgetPassword}";
            await emailService.sendEmailAsync(user.Email,subject,message);
            return Ok(new ApiResponse(200,
                  "Password rreset link has been send to your email .. check your email :)"));
            
        }
        
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
        {
            if(ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null) 
                    return NotFound(new ApiResponse(404, "Email incorrect"));
                if(string.Compare(model.newPassword,model.confirmNewPassword) !=0)
                    return BadRequest(new ApiResponse(400, "Password not match"));

                if(string.IsNullOrEmpty(model.Token))
                    return BadRequest(new ApiResponse(400, "Token inValid"));

                var result = await userManager.ResetPasswordAsync
                    (user,model.Token,model.newPassword);

                if (result.Succeeded)
                {
                    return Ok(new ApiResponse(200,"Reseting successfully"));
                }
                else
                {
                    return BadRequest(new ApiResponse(400, "error while reseting"));
                }
            }
            return BadRequest(new ApiResponse(400, "Check your info"));

        }

        [HttpPost("reset-token")]
        public async Task<IActionResult> TokenToResetPassword([FromBody] string email)
        {
            var user = await userManager.FindByEmailAsync (email);
            if (user == null)
                return NotFound(new ApiResponse(404));
            var token = userManager.GeneratePasswordResetTokenAsync(user);
            return Ok(new { token = token });
        }
    }
}
