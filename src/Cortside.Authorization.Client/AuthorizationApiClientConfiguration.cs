using Cortside.RestApiClient.Authenticators.OpenIDConnect;

namespace Cortside.Authorization.Client {
    public class AuthorizationApiClientConfiguration {
        public string ServiceUrl { get; set; }
        public TokenRequest Authentication { get; set; }
        public Guid PolicyResourceId { get; set; }
        public IEnumerable<string> ClaimTypes { get; set; } = [];
        public TimeSpan? CacheDuration { get; set; }
        public bool CacheEnabled => CacheDuration.HasValue;
    }
}
