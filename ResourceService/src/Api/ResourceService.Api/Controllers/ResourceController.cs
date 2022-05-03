using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ResourceService.Application.Features.Image.Queries.GetAllImages;
using ResourceService.Application.Features.Image.Queries.GetImageById;
using ResourceService.Application.Exceptions.Exceptions;
using ResourceService.Application.Features.Image.Queries.Common;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using ResourceService.Application.Features.Image.Commands.CreateImage;
using ResourceService.Application.Features.Image.Commands.DeleteImageById;
using ResourceService.Application.Features.Image.Commands.EditImageById;
using ResourceService.Application.Features.Image.Commands.PartiallyEditImageById;

namespace ResourceService.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ErrorMessage), 500)]
    [ProducesResponseType(typeof(ErrorMessage), 401)]
    [Route("api/v1")]
    public class ResourceController : ControllerBase
    {
        public readonly IMediator mediator;
        public ResourceController(IMediator mediator) => this.mediator = mediator;

        /// <summary>
        /// List all images
        /// </summary>
        /// <returns>Gets a list of all images for the authenticated user.</returns>
        /// <response code="200">Successful operation.</response>
        /// <response code="401">Access token invalid or not provided.</response>
        /// <response code="500">An unexpected condition was encountered.</response>
        [ProducesResponseType(typeof(List<ImageDto>), 200)]
        [HttpGet("images")]
        public async Task<IActionResult> GetAllImagesAsync()
        {
            return Ok(await mediator.Send(new GetAllImagesQuery()));
        }

        /// <summary>
        /// Create image
        /// </summary>
        /// <returns>Creates a new image owned by the authenticated user.</returns>
        /// <response code="201">The request has been fulfilled, resulting in the creation of a new image.</response>
        /// <response code="400">The request cannot or will not be processed due to something that is perceived to be a client error (for example validation error).</response>
        /// <response code="401">Access token invalid or not provided.</response>
        /// <response code="500">An unexpected condition was encountered.</response>
        [ProducesResponseType(typeof(CreateImageResponse), 201)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [HttpPost("images")]
        public async Task<IActionResult> CreateImage([FromBody] CreateImageControllerRequest request)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            return new ObjectResult(await mediator.Send(new CreateImageCommand
            {
                Data = request.Data,
                Description = request.Description,
                ContentType = request.ContentType,
                Location = request.Location,
                UserId = GetUserId(token)
            }))
            { StatusCode = 201 };
        }

        /// <summary>
        /// Get single image
        /// </summary>
        /// <returns>Gets a specific image.</returns>
        /// <response code="200">Returns an specific image.</response>
        /// <response code="401">Access token invalid or not provided.</response>
        /// <response code="403">The request contained valid data and was understood by the server, but the server is refusing action due to the authenticated user not having the necessary permissions for the resource.</response>
        /// <response code="404">The requested resource was not found.</response>
        /// <response code="500">An unexpected condition was encountered.</response>
        [ProducesResponseType(typeof(ImageDto), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 403)]
        [ProducesResponseType(typeof(ErrorMessage), 404)]
        [HttpGet("images/{id}")]
        public async Task<IActionResult> GetImageByIdAsync([FromRoute] string id)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            return Ok(await mediator.Send(new GetImageByIdQuery
            {
                Id = id,
                UserId = GetUserId(token)
            }));
        }

        /// <summary>
        /// Update image
        /// </summary>
        /// <returns>Updates an existing image.</returns>
        /// <response code="204">The server successfully processed the request and is not returning any content.</response>
        /// <response code="400">The request cannot or will not be processed due to something that is perceived to be a client error (for example validation error).</response>
        /// <response code="401">Access token invalid or not provided.</response>
        /// <response code="403">The request contained valid data and was understood by the server, but the server is refusing action due to the authenticated user not having the necessary permissions for the resource.</response>
        /// <response code="404">The requested resource was not found.</response>
        /// <response code="500">An unexpected condition was encountered.</response>
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(typeof(ErrorMessage), 403)]
        [ProducesResponseType(typeof(ErrorMessage), 404)]
        [HttpPut("images/{id}")]
        public async Task<IActionResult> EditImageById([FromRoute] string id, [FromBody] EditImageByIdControllerRequest request)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            await mediator.Send(new EditImageByIdCommand
            {
                Id = id,
                Data = request.Data,
                ContentType = request.ContentType,
                Description = request.Description,
                Location = request.Location,
                UserId = GetUserId(token)
            });
            return NoContent();
        }

        /// <summary>
        /// Partially updates image
        /// </summary>
        /// <returns>Partially updates an existing image.</returns>
        /// <response code="204">The server successfully processed the request and is not returning any content.</response>
        /// <response code="400">The request cannot or will not be processed due to something that is perceived to be a client error (for example validation error).</response>
        /// <response code="401">Access token invalid or not provided.</response>
        /// <response code="403">The request contained valid data and was understood by the server, but the server is refusing action due to the authenticated user not having the necessary permissions for the resource.</response>
        /// <response code="404">The requested resource was not found.</response>
        /// <response code="500">An unexpected condition was encountered.</response>
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(typeof(ErrorMessage), 403)]
        [ProducesResponseType(typeof(ErrorMessage), 404)]
        [HttpPatch("images/{id}")]
        public async Task<IActionResult> PartiallyEditImageById([FromRoute] string id, [FromBody] PartiallyEditImageByIdControllerRequest request)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            await mediator.Send(new PartiallyEditImageByIdCommand
            {
                Id = id,
                Description = request.Description,
                UserId = GetUserId(token)
            });
            return NoContent();
        }

        /// <summary>
        /// Delete image
        /// </summary>
        /// <returns>Deletes an image.</returns>
        /// <response code="204">The server successfully processed the request and is not returning any content.</response>
        /// <response code="401">Access token invalid or not provided.</response>
        /// <response code="403">The request contained valid data and was understood by the server, but the server is refusing action due to the authenticated user not having the necessary permissions for the resource.</response>
        /// <response code="404">The requested resource was not found.</response>
        /// <response code="500">An unexpected condition was encountered.</response>
        [ProducesResponseType(typeof(ErrorMessage), 403)]
        [ProducesResponseType(typeof(ErrorMessage), 404)]
        [HttpDelete("images/{id}")]
        public async Task<IActionResult> DeleteImageById([FromRoute] string id)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            await mediator.Send(new DeleteImageByIdCommand
            {
                Id = id,
                UserId = GetUserId(token)
            });
            return NoContent();
        }

        private static Guid GetUserId(string accessToken)
        {
            var securityTokenHandler = new JwtSecurityTokenHandler();
            if (securityTokenHandler.CanReadToken(accessToken))
            {
                var decriptedToken = securityTokenHandler.ReadJwtToken(accessToken);
                var claims = decriptedToken.Claims;
                //At this point you can get the claims in the token, in the example I am getting the expiration date claims
                //this step depends of the claims included at the moment of the token is generated
                //and what you are trying to accomplish
                return Guid.Parse(claims.Where(c => c.Type == "uid").FirstOrDefault().Value);
            }
            return Guid.NewGuid();
        }
    }
}