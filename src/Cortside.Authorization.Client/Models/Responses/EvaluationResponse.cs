namespace Cortside.Authorization.Client.Models.Responses {
    public class EvaluationResponse {
        public IEnumerable<string> Roles { get; set; } = Array.Empty<string>();
        public IEnumerable<string> Permissions { get; set; } = Array.Empty<string>();
    }
}
