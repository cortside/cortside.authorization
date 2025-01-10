#pragma warning disable CS1591 // Missing XML comments

using System;
using Cortside.Authorization.Dto;
using Cortside.Authorization.WebApi.Models;
using Cortside.Authorization.WebApi.Models.Requests;
using Cortside.Authorization.WebApi.Models.Responses;

namespace Cortside.Authorization.WebApi.Mappers {
    public class PolicyModelMapper {

        public PolicyModelMapper() {
        }



        internal EvaluatePolicyDto MapToDto(EvaluatePolicyRequest request, Guid resourceId) {
            return new EvaluatePolicyDto {
                PolicyResourceId = resourceId,
                Claims = request.Claims.ConvertAll(Map),
            };
        }

        internal ClaimDto Map(ClaimModel model) {
            return new ClaimDto {
                Type = model.Type,
                Value = model.Value
            };
        }

        internal AuthorizationModel Map(AuthorizationDto dto) {
            return new AuthorizationModel {
                Roles = dto.Roles,
                Permissions = dto.Permissions
            };
        }
    }
}
