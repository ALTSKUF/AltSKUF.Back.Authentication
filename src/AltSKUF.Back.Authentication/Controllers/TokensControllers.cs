using AltSKUF.Back.Authentication.Domain.Extensions;
using AltSKUF.Back.Authentication.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AltSKUF.Back.Authentication.Controllers
{
    [ApiController]
    [Route("/Tokens")]
    public class TokensControllers(
        IAuthenticationService authenticationService) : Controller
    {
        [HttpGet("Services")]
        public IActionResult GetServiceToken()
        {
            try { return Ok(authenticationService.GetServiceToken()); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [Authorize("Services")]
        [HttpGet("Get")]
        public IActionResult GetTokens(
            [FromQuery] Guid userId)
        {
            try { return Ok(authenticationService.GetTokens(userId)); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [Authorize("Refresh")]
        [HttpGet("Refresh")]
        public IActionResult RefreshTokens()
        {
            var userId = User.Claims.GetUserId();

            try { return Ok(authenticationService.GetTokens(userId)); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}