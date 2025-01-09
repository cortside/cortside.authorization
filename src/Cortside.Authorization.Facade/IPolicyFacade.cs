using System.Threading.Tasks;
using Cortside.Authorization.Dto;

namespace Cortside.Authorization.Facade {
    public interface IPolicyFacade {
        Task<AuthorizationDto> EvaluatePolicyAsync(EvaluatePolicyDto dto);
    }
}
