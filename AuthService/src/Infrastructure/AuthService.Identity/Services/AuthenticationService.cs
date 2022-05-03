using AuthService.Application.Contracts.Identity;
using AuthService.Application.Exceptions;
using AuthService.Application.Models.Identity;
using AuthService.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace AuthService.Identity.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtSettings _jwtSettings;

        public AuthenticationService(UserManager<ApplicationUser> userManager,
            IOptions<JwtSettings> jwtSettings,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _signInManager = signInManager;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            var validator = new LoginValidator();
            var validationResult = await validator.ValidateAsync(request);
            if (validationResult.Errors.Count > 0)
                throw new HttpResponseException(400, new ErrorMessage { StatusCode = 400, Message = "The request cannot or will not be processed due to something that is perceived to be a client error (for example validation error)." });

            var user = await _userManager.FindByNameAsync(request.Username);

            if (user == null)
                throw new HttpResponseException(401, new ErrorMessage { StatusCode = 401, Message = "Credentials invalid or not provided." });

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);

            if (!result.Succeeded)
                throw new HttpResponseException(401, new ErrorMessage { StatusCode = 401, Message = "Credentials invalid or not provided." });

            try
            {
                JwtSecurityToken jwtSecurityToken = await GenerateToken(user);

                var response = new AuthenticationResponse
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                };
                return response;
            }
            catch (Exception ex) when (ex is not HttpResponseException)
            {
                throw new HttpResponseException(500, new ErrorMessage { StatusCode = 500, Message = "An unexpected condition was encountered." });
            }
        }

        public async Task<RegistrationResponse> RegisterAsync(RegistrationRequest request)
        {
            try
            {

                var validator = new RegisterValidator();
                var validationResult = await validator.ValidateAsync(request);
                if (validationResult.Errors.Count > 0)
                    throw new HttpResponseException(400, new ErrorMessage { StatusCode = 400, Message = "The request cannot or will not be processed due to something that is perceived to be a client error (for example validation error)." });

                var existingUser = await _userManager.FindByNameAsync(request.Username);

                if (existingUser != null)
                    throw new HttpResponseException(409, new ErrorMessage { StatusCode = 409, Message = "The username and/or email address is already registered." });

                var user = new ApplicationUser
                {
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    UserName = request.Username,
                    EmailConfirmed = true
                };

                var existingEmail = await _userManager.FindByEmailAsync(request.Email);

                if (existingEmail != null) throw new HttpResponseException(409, new ErrorMessage { StatusCode = 409, Message = "The username and/or email address is already registered." });

                var result = await _userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                    throw new HttpResponseException(400, new ErrorMessage { StatusCode = 400, Message = "The request cannot or will not be processed due to something that is perceived to be a client error (for example validation error)." });

                return new RegistrationResponse() { Id = user.Id };
            }
            catch (Exception ex) when (ex is not HttpResponseException)
            {
                throw new HttpResponseException(500, new ErrorMessage { StatusCode = 500, Message = "An unexpected condition was encountered." });
            }
        }

        private async Task<JwtSecurityToken> GenerateToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var privateKey = System.IO.File.ReadAllText(@"./private.pem");
            var rsa = RSA.Create();
            rsa.ImportFromPem(privateKey.ToCharArray());

            var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256)
            {
                CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false }
            };

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }
    }
}