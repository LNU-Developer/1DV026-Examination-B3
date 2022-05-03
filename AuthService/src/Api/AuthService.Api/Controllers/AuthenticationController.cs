using AuthService.Application.Contracts.Identity;
using AuthService.Application.Exceptions;
using AuthService.Application.Models.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Api.Controllers
{
    [Produces("application/json")]
    [ProducesResponseType(typeof(ErrorMessage), 500)]
    [Route("api/v1")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        public AuthenticationController(IAuthenticationService authenticationService) => _authService = authenticationService;

        /// <summary>
        /// Registers a new account
        /// </summary>
        /// <returns>Registers a new account for the user.</returns>
        /// <response code="201">The request has been fulfilled, resulting in the creation of a new account.</response>
        /// <response code="400">The request cannot or will not be processed due to something that is perceived to be a client error (for example validation error).</response>
        /// <response code="409">The username and/or email address is already registered.</response>
        /// <response code="500">An unexpected condition was encountered.</response>
        [ProducesResponseType(typeof(RegistrationResponse), 201)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(typeof(ErrorMessage), 409)]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegistrationRequest request)
        {
            return Ok(await _authService.RegisterAsync(request));
        }

        /// <summary>
        /// Login an user
        /// </summary>
        /// <returns>Logs in a registered user to get a new JWT generated.</returns>
        /// <response code="200">The request has been fulfilled, resulting in a generated JWT.</response>
        /// <response code="401">Credentials invalid or not provided.</response>
        /// <response code="500">An unexpected condition was encountered.</response>
        [ProducesResponseType(typeof(AuthenticationResponse), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 401)]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] AuthenticationRequest request)
        {
            return Ok(await _authService.AuthenticateAsync(request));
        }
    }
}