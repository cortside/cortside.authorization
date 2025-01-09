#pragma warning disable CS1591 // Missing XML comments

using Cortside.Authorization.WebApi.Mappers;
using Cortside.Common.BootStrap;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cortside.Authorization.WebApi.Installers {
    public class ModelMapperInstaller : IInstaller {
        public void Install(IServiceCollection services, IConfiguration configuration) {
            services.AddSingletonClassesBySuffix<PolicyModelMapper>("Mapper");
        }
    }
}
