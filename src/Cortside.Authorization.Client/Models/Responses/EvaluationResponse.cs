namespace Cortside.Authorization.Client.Models.Responses {
    public class EvaluationResponse {
        public IEnumerable<string> Roles { get; set; } = [];
        public IEnumerable<string> Permissions { get; set; } = [];
    }
}
