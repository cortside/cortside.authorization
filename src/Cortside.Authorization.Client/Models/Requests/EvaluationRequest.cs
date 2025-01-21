using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace Cortside.Authorization.Client.Models.Requests {
    public class EvaluationRequest {

        public IEnumerable<ClaimModel> Claims { get; set; } = [];

        public string GetCacheKey(Guid policyResourceId) {
            var sorted = JsonConvert.SerializeObject(SortClaims(Claims));
            string input = $"{sorted}";

            //Convert the string into an array of bytes.
            byte[] messageBytes = Encoding.UTF8.GetBytes(input);

            //Create the hash value from the array of bytes.
            byte[] hashValue = SHA256.HashData(messageBytes);

            return $"policy:{policyResourceId}:" + Convert.ToHexString(hashValue);
        }

        private static IEnumerable<ClaimModel> SortClaims(IEnumerable<ClaimModel> claimsList) {
            return claimsList.OrderBy(c => c.Type).ThenBy(x => x.Value);
        }


    }
}
