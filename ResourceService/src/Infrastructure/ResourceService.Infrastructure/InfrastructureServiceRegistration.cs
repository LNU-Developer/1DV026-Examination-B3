using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ResourceService.Application.Contracts;
using ResourceService.Application.Exceptions.Exceptions;
using ResourceService.Application.Models.Identity;
using ResourceService.Infrastructure.Image;

namespace ResourceService.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                var publicKey = System.IO.File.ReadAllText(@"./public.pem");
                var rsa = RSA.Create();
                rsa.ImportFromPem(publicKey.ToCharArray());

                options.IncludeErrorDetails = false;
                options.RequireHttpsMetadata = false;
                // Configure the actual Bearer validation
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new RsaSecurityKey(rsa),
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    RequireSignedTokens = true,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true
                };

                options.Events = new JwtBearerEvents()
                {
                    OnChallenge = async c =>
                    {
                        c.HandleResponse();
                        var message = JsonConvert.SerializeObject(new ErrorMessage
                        {
                            StatusCode = 401,
                            Message = "Access token invalid or not provided."
                        });
                        c.Response.ContentType = "application/json";
                        c.Response.StatusCode = 401;
                        await c.Response.WriteAsync(message);
                        return;
                    }
                };
            });

            services.AddHttpClient<IImageServiceClient, ImageServiceClient>(c =>
            {
                var token = System.IO.File.ReadAllText(@"./ImageAccessToken.pem");
                c.BaseAddress = new Uri("https://courselab.lnu.se/picture-it/images/api/v1/");
                c.DefaultRequestHeaders.Add("X-API-Private-Token", token);
            });

            return services;
        }
    }
}