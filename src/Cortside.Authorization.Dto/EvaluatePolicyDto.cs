using System.Collections.Generic;

namespace Cortside.Authorization.Dto {
    public class EvaluatePolicyDto {
        public string PolicyName { get; set; }
        public List<ClaimDto> Claims { get; set; } = [];
    }
}
