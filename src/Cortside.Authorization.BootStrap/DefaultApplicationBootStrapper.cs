using Cortside.Authorization.BootStrap.Installer;
using Cortside.Common.BootStrap;

namespace Cortside.Authorization.BootStrap {
    public class DefaultApplicationBootStrapper : BootStrapper {
        public DefaultApplicationBootStrapper() {
            installers = [
                new RepositoryInstaller(),
                new DomainServiceInstaller(),
                new DistributedLockInstaller(),
                new FacadeInstaller()
            ];
        }
    }
}
