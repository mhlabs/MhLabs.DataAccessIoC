using Microsoft.Extensions.DependencyInjection;
using System;
using model_project.DataAccess;
using model_project.DataContracts;
using Moq;
using Xunit;
using MhLabs.DataAccessIoC.IoC;
using MhLabs.DataAccessIoC.Resolvers;
using Xunit.Sdk;

namespace MhLabs.DataAccessIoc.Tests
{
    
    public class ServiceCollectionTests
    {
        [Fact]
        public void ServiceCollectionExtension_ShouldRegisterCorrectType()
        {
            var mock = new Mock<IServiceCollection>();
//            mock.Object.AddAWSRepository<ProductRepository>().WithTableNameFrom<EnvironmentVariableResourceResolver>("Test");


        }
    }
}
