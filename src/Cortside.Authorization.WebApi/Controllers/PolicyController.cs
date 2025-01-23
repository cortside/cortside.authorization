using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Cortside.Authorization.Facade;
using Cortside.Authorization.WebApi.Mappers;
using Cortside.Authorization.WebApi.Models.Requests;
using Cortside.Authorization.WebApi.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cortside.Authorization.WebApi.Controllers {
    /// <summary>
    /// Represents the shared functionality/resources of the policy resource
    /// </summary>
    [ApiVersion("1")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    [Route("api/v{version:apiVersion}/policies")]
    public class PolicyController : ControllerBase {
        private readonly IPolicyFacade facade;
        private readonly PolicyModelMapper policyMapper;

        /// <summary>
        /// Initializes a new instance of the PolicyController
        /// </summary>
        public PolicyController(IPolicyFacade facade, PolicyModelMapper policyMapper) {
            this.facade = facade;
            this.policyMapper = policyMapper;
        }

        /// <summary>
        /// Evaluates policy for user claims
        /// </summary>
        [HttpPost("{resourceId}/evaluate")]
        [ProducesResponseType(typeof(AuthorizationModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> EvaluatePolicysAsync(Guid resourceId, [FromBody] EvaluatePolicyRequest request) {
            var dto = policyMapper.MapToDto(request, resourceId);
            var result = await facade.EvaluatePolicyAsync(dto).ConfigureAwait(false);
            var model = policyMapper.Map(result);
            return Ok(model);
        }


    }
}
