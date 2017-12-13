using System;
using System.Collections.Generic;
using System.Text;
using Amazon;

namespace MhLabs.DataAccessIoC.AWS
{
    public static class Region
    {
        public static RegionEndpoint Current = RegionEndpoint.GetBySystemName(Environment.GetEnvironmentVariable("AWS_DEFAULT_REGION"));
    }
}
