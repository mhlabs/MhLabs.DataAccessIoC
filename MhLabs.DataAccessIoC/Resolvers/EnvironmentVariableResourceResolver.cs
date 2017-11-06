using System;
using System.Collections.Generic;
using System.Text;

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
