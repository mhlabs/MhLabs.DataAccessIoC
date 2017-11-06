using MhLabs.DataAccessIoC.Resolvers;

namespace MhLabs.DataAccessIoC.IoC
{
    public class RepositoryOptions
    {
        public RepositoryOptions()
        {
            ResourceResolver = new EnvironmentVariableResourceResolver();
        }

        public IResourceResolver ResourceResolver { get; set; }
    }
}