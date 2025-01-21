namespace Cortside.Authorization.Client.Models.Responses {
    public class EvaluationResponse {
        public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();
        public IEnumerable<string> Permissions { get; set; } = Enumerable.Empty<string>();
    }
}
