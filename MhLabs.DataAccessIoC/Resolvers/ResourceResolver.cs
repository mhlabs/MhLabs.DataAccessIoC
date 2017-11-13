using System;
using System.Collections.Generic;
using System.Text;

namespace MhLabs.DataAccessIoC.Resolvers
{
    public static class ResourceResolver
    {
        public static IResourceResolver Current { get; set; } = new EnvironmentVariableResourceResolver();
    }
}
