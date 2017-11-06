using MhLabs.DataAccessIoC.Resolvers;

namespace MhLabs.DataAccessIoC.AWS
{
    public interface IAWSRepository
    {
        IResourceResolver ResourceResolver { get; set; }
    }
}