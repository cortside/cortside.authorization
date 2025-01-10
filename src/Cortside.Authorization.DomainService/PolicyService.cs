using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cortside.Authorization.Data.Repositories;
using Cortside.Authorization.Domain.Entities;
using Cortside.Authorization.Dto;
using Microsoft.Extensions.Logging;

namespace Cortside.Authorization.DomainService {
    public class PolicyService : IPolicyService {
        private readonly ILogger<PolicyService> logger;
        private readonly IPolicyRepository policyRepository;

        public PolicyService(IPolicyRepository policyRepository, ILogger<PolicyService> logger) {
            this.logger = logger;
            this.policyRepository = policyRepository;
        }

        public async Task<IList<Role>> GetRolesByClaimsAsync(EvaluatePolicyDto dto) {
            logger.LogDebug("getting roles by claims for policy {Policy}", dto.PolicyResourceId);
            //Guard.Against(() => dto.Claims.Where(x => x.Type == "iss" && x.Value == config.Authority)); // need this?
            var claims = dto.Claims.Select(x => new KeyValuePair<string, string>(x.Type, x.Value));
            var entityList = await policyRepository.GetRolesByClaimsAsync(dto.PolicyResourceId, claims).ConfigureAwait(false);
            return entityList;
        }


    }
}
