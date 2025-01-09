namespace Cortside.Authorization.WebApi.Models {
    /// <summary>
    /// Simple claim model
    /// </summary>
    public class ClaimModel {
        /// <summary>
        /// Type of claim
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Value of claim
        /// </summary>
        public string Value { get; set; }
    }
}
