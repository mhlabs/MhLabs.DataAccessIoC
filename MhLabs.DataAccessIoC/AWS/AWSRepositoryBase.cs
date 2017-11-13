using Amazon.Runtime;
using MhLabs.DataAccessIoC.Resolvers;
namespace MhLabs.DataAccessIoC.AWS
{
    public abstract class AWSRepositoryBase<T> : IAWSRepository where T : IAmazonService 
    {
        protected AWSRepositoryBase(T dataAccessClient)
        {
            DataAccessClient = dataAccessClient;
        }

        protected T DataAccessClient { get; }

        /// <summary>
        /// For example the name of the environment variable containing the table name
        /// </summary>
        protected abstract string AWSResourceKey { get; }

        protected string ResourceName => ResourceResolver.Current.Resolve(AWSResourceKey);

    }
}
