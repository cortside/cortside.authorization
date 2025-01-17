using System.Security.Claims;
using Cortside.Authorization.Client.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Cortside.Authorization.Client.Tests {
    public class ModelsTest {


        [Fact]
        public void ShouldMapAllClaimTypes() {
            // arrange
            List<Claim> claims = [new Claim("sub", "jane"), new Claim("group", "admin")];
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims));
            var dto = new EvaluationDto { User = user };
            var client = new Client.AuthorizationApiClient(new AuthorizationApiClientConfiguration { ServiceUrl = "http://blah" }, // no claimtypes specified
                new Mock<ILogger<Client.AuthorizationApiClient>>().Object,
                new Mock<IHttpContextAccessor>().Object
                );

            // act
            var result = client.Map(dto);

            // assert
            result.Should().NotBeNull();
            result.Claims.Should().NotBeNullOrEmpty().And.HaveCount(claims.Count);
        }

        [Fact]
        public void ShouldMapOnlyDefinedClaimTypes() {
            // arrange
            List<Claim> claims = [new Claim("sub", "jane"), new Claim("group", "admin")];
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims));
            var dto = new EvaluationDto { User = user };
            var client = new Client.AuthorizationApiClient(new AuthorizationApiClientConfiguration { ServiceUrl = "http://blah", ClaimTypes = ["sub"] }, // claimtypes specified
                new Mock<ILogger<Client.AuthorizationApiClient>>().Object,
                new Mock<IHttpContextAccessor>().Object
                );

            // act
            var result = client.Map(dto);

            // assert
            result.Should().NotBeNull();
            result.Claims.Should().NotBeNullOrEmpty().And.HaveCount(1);
            result.Claims.Should().OnlyContain(c => c.Type == "sub");
        }

        [Fact]
        public void ShouldGetCacheKey() {
            // arrange
            List<Claim> claims = [new Claim("sub", "jane"), new Claim("group", "admin")];
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims));
            var dto = new EvaluationDto { User = user };
            var client = new Client.AuthorizationApiClient(new AuthorizationApiClientConfiguration { ServiceUrl = "http://blah" },
                new Mock<ILogger<Client.AuthorizationApiClient>>().Object,
                new Mock<IHttpContextAccessor>().Object
                );

            var requestModel = client.Map(dto);
            var policyId = Guid.NewGuid();

            // act
            var cacheKey = requestModel.GetCacheKey(policyId);

            // assert
            cacheKey.Should().NotBeNullOrEmpty();
            cacheKey.Should().StartWith($"policy:{policyId}:");
            var hashed = cacheKey.Split(":")[2];
            hashed.Should().NotBeNullOrEmpty().And.Be("2ED92C16F9416115B23E47A6AE41FEF41435C86D9D5A55E76655C39EEDFF5964");
        }

    }
}
