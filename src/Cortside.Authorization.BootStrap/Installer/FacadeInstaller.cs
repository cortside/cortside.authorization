using Cortside.Authorization.Facade;
using Cortside.Authorization.Facade.Mappers;
using Cortside.Common.BootStrap;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cortside.Authorization.BootStrap.Installer {
    public class FacadeInstaller : IInstaller {
        public void Install(IServiceCollection services, IConfiguration configuration) {
            services.AddScopedInterfacesBySuffix<PolicyFacade>("Facade");
            services.AddSingletonClassesBySuffix<PolicyMapper>("Mapper");
        }
    }
}
