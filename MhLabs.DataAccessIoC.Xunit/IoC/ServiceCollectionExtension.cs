using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MhLabs.DataAccessIoC.Abstraction;
using MhLabs.DataAccessIoC.AWS;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace MhLabs.DataAccessIoC.Xunit.IoC
{
    public static class ServiceCollectionExtension
    {

        public static void AddRepositoryMocks(this IServiceCollection services, Assembly assembly)
        {
            var repos = GetImplementations(typeof(IAWSRepository), assembly).Where(p => !p.GetTypeInfo().IsAbstract);
            foreach (var repo in repos)
            {
                var ctors = repo.GetConstructors();
                if (ctors.Length != 1)
                {
                    throw new ArgumentException("An autowired repository needs exactly one constructor");
                }
                repo.GetInterfaces().ToList().ForEach(i =>
                {
                    var mock = Activator.CreateInstance(typeof(Mock<>).MakeGenericType(i)) as Mock;
                    services.AddSingleton(mock.GetType(), h => mock);
                    services.AddSingleton(i, h => mock?.Object);
                });
            }            
        }

        private static IEnumerable<Type> GetImplementations(Type interfaceType, Assembly assembly)
        {
            var ass = assembly.DefinedTypes;                
            foreach (var ti in ass)
            {
                if (ti.ImplementedInterfaces.Contains(interfaceType))
                {
                    yield return ti.AsType();
                }
            }
        }
    }
}
