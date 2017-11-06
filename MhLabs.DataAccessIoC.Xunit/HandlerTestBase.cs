using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using MhLabs.DataAccessIoC.IoC;
using MhLabs.DataAccessIoC.Xunit.IoC;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace MhLabs.DataAccessIoC.Xunit
{
    public abstract class HandlerTestBase<THandler>
    {
        private IServiceCollection Services;
        private IServiceProvider _serviceProvider;

        protected HandlerTestBase()
        {
            Services = new ServiceCollection();
            var assName = typeof(THandler).AssemblyQualifiedName.Split(' ')[1].Trim(',');
            var assembly = Assembly.Load(new AssemblyName(assName));
            Services.AddHandlers(assembly);
            Services.AddRepositoryMocks(assembly);
           _serviceProvider = Services.BuildServiceProvider();
        }

        private Mock<T> GetMock<T>() where T : class
        {            
            return _serviceProvider.GetService<Mock<T>>();
        }

        protected THandler Handler => _serviceProvider.GetService<THandler>();

        protected void SetupMockFor<T>(Action<Mock<T>> setup, bool reset = true) where T : class
        {
            var mock = GetMock<T>();
            if (reset)
            {
                mock.Reset();
            }
            setup(mock);
        }
        protected void ModifyMockFor<T>(Action<Mock<T>> setup) where T : class
        {
            SetupMockFor(setup, false);
        }
    }
}
