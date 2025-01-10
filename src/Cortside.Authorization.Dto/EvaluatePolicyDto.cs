using System;
using System.Collections.Generic;

namespace Cortside.Authorization.Dto {
    public class EvaluatePolicyDto {
        public List<ClaimDto> Claims { get; set; } = [];
        public Guid PolicyResourceId { get; set; }
    }
}
