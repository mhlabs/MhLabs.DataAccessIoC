using System;
using System.Collections.Generic;
using System.Text;

namespace MhLabs.DataAccessIoC.Resolvers
{
    public interface IResourceResolver
    {
        string Resolve(string key);
    }
}
