using System;
using Cortside.AspNetCore.Common.Dtos;

namespace Cortside.Authorization.Dto {
    public class PolicyDto : AuditableEntityDto {
        public int PolicyId { get; set; }
        public Guid PolicyResourceId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
