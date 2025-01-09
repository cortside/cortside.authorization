using System.Threading.Tasks;
using Cortside.AspNetCore.EntityFramework;
using Cortside.Authorization.DomainService;
using Cortside.Authorization.Dto;
using Cortside.Authorization.Facade.Mappers;

namespace Cortside.Authorization.Facade {
    public class PolicyFacade : IPolicyFacade {
        private readonly IUnitOfWork uow;
        private readonly IPolicyService policyService;
        private readonly PolicyMapper mapper;

        public PolicyFacade(IUnitOfWork uow, IPolicyService policyService, PolicyMapper mapper) {
            this.uow = uow;
            this.policyService = policyService;
            this.mapper = mapper;
        }



        public async Task<AuthorizationDto> EvaluatePolicyAsync(EvaluatePolicyDto dto) {
            // get list of Roles that match policyname and policyroleclaim(s)
            using (await uow.BeginReadUncommitedAsync().ConfigureAwait(false)) {
                var roles = await policyService.GetRolesByClaimsAsync(dto).ConfigureAwait(false);
                return mapper.MapToDto(roles);
            }
        }
    }
}
