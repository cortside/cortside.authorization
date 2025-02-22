using System.Threading.Tasks;
using Cortside.AspNetCore.Builder;

namespace Cortside.Authorization.WebApi {
    /// <summary>
    /// Program
    /// </summary>
    public static class Program {
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static Task<int> Main(string[] args) {
            var builder = WebApiHost.CreateBuilder(args)
                .UseStartup<Startup>();

            var api = builder.Build();
            return api.StartAsync();
        }
    }
}
