using System;
using System.Collections.Generic;
using System.Text;

namespace MhLabs.DataAccessIoC.Resolvers
{
    public class HardCodedNameResolver : IResourceResolver
    {
        public string Resolve(string key)
        {
            return key;
        }
    }
}
