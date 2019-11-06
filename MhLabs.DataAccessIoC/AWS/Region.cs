using System;
using Amazon;

namespace MhLabs.DataAccessIoC.AWS
{
    public static class Region
    {
        // AWS_REGION is the correct environmentvariable to check.
        // Fallback to AWS_DEFAULT_REGION as to not break backwards compatibility
        public static RegionEndpoint Current { get; } = RegionEndpoint.GetBySystemName(
            Environment.GetEnvironmentVariable("AWS_REGION") ?? Environment.GetEnvironmentVariable("AWS_DEFAULT_REGION"));
    }
}
