using System.Collections.Generic;
using System.Threading.Tasks;
using Cortside.Authorization.Domain.Entities;
using Cortside.Authorization.Dto;

namespace Cortside.Authorization.DomainService {
    public interface IPolicyService {

        Task<IList<Role>> GetRolesByClaimsAsync(EvaluatePolicyDto dto);
    }
}
