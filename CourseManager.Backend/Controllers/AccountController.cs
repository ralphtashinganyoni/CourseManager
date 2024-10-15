using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using CourseManager.Domain.Entities;
using CourseManager.Backend.DTOs;

namespace CourseManager.Backend.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        // Login endpoint
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                return Ok();
            }
            return Unauthorized("Invalid login attempt.");
        }

        // Logout endpoint
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return Ok();
        }

        // Register endpoint
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var user = new User 
            {
                UserName = model.Email, 
                Email = model.Email, 
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Address = model.PhysicalAddress
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogInformation("User registered and logged in.");
                return Ok();
            }
            return BadRequest(result.Errors);
        }

        // Confirm Email endpoint
        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found.");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded ? Ok("Email confirmed.") : BadRequest("Invalid email confirmation token.");
        }

        // Resend Confirmation Email endpoint
        [HttpPost("resendConfirmationEmail")]
        public async Task<IActionResult> ResendConfirmationEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound("User not found.");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            // Send confirmation email here (not implemented in this example)
            return Ok("Confirmation email resent.");
        }

        // Forgot Password endpoint
        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return NotFound("User not found.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            // Send password reset email here (not implemented in this example)
            return Ok("Password reset link sent.");
        }

        // Reset Password endpoint
        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return NotFound("User not found.");

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            return result.Succeeded ? Ok("Password reset successful.") : BadRequest(result.Errors);
        }

        // Manage User Info endpoint (GET)
        [HttpGet("manage/info")]
        [Authorize]
        public async Task<IActionResult> GetUserInfo()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound("User not found.");

            return Ok(new { user.UserName, user.Email });
        }

        // Manage User Info endpoint (POST - update)
        [HttpPost("manage/info")]
        [Authorize]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UpdateUserInfoModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound("User not found.");

            user.Email = model.Email;
            user.UserName = model.Username;
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded ? Ok("User info updated.") : BadRequest(result.Errors);
        }

        // Enable/Disable 2FA endpoint
        [HttpPost("manage/2fa")]
        [Authorize]
        public async Task<IActionResult> ManageTwoFactor(bool enable)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound("User not found.");

            if (enable)
                await _userManager.SetTwoFactorEnabledAsync(user, true);
            else
                await _userManager.SetTwoFactorEnabledAsync(user, false);

            return Ok($"Two-factor authentication {(enable ? "enabled" : "disabled")}.");
        }
    }
}
