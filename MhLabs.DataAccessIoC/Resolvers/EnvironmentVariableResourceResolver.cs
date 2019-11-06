using System;

namespace MhLabs.DataAccessIoC.Resolvers
{
    public class EnvironmentVariableResourceResolver : IResourceResolver
    {
        public string Resolve(string key)
        {
            return Environment.GetEnvironmentVariable(key);
        }
    }
}
