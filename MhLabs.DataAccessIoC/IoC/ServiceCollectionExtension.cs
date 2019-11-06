using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MhLabs.DataAccessIoC.Abstraction;
using MhLabs.DataAccessIoC.AWS;
using Microsoft.Extensions.DependencyInjection;

namespace MhLabs.DataAccessIoC.IoC
{
    public static class ServiceCollectionExtension
    {
        public static void AddHandlers<TAssemblyType>(this IServiceCollection services)
        {
            var assName = typeof(TAssemblyType).GetTypeInfo().Assembly.FullName;

            AddHandlers(services, Assembly.Load(new AssemblyName(assName)));
        }

        public static void AddHandlers(this IServiceCollection services, Assembly assembly)
        {
            var handlers = GetImplementations(typeof(IHandler), assembly).Where(p => !p.GetTypeInfo().IsAbstract);
            foreach (var handler in handlers)
            {
                var ctors = handler.GetConstructors();
                if (ctors.Length != 1)
                {
                    throw new ArgumentException("An autowired handler needs exactly one constructor");
                }
                var ctor = ctors.First();
                var parameters = ctor.GetParameters();
                services.AddSingleton(handler, h =>
                {
                    var args = parameters.Select(p => h.GetService(p.ParameterType));
                    var obj = ctor.Invoke(args.ToArray());
                    return obj;
                });
            }
        }

        public static void AddAWSRepositories<TAssemblyType>(this IServiceCollection services)
        {
            var assName = typeof(TAssemblyType).GetTypeInfo().Assembly.FullName;
            var assembly = Assembly.Load(new AssemblyName(assName));
            AddAWSRepositories(services, assembly);
        }

        public static void AddAWSRepositories(this IServiceCollection services, Assembly assembly)
            {
            var repos = GetImplementations(typeof(IAWSRepository), assembly).Where(p => !p.GetTypeInfo().IsAbstract);
            foreach (var repo in repos)
            {
                var ctors = repo.GetConstructors();
                if (ctors.Length != 1)
                {
                    throw new ArgumentException("An autowired repository needs exactly one constructor");
                }
                var ctor = ctors.First();
                var parameters = ctor.GetParameters();
                repo.GetInterfaces().ToList().ForEach(i =>
                    services.AddSingleton(i, h =>
                    {
                        var args = parameters.Select(p => h.GetService(p.ParameterType));
                        var obj = ctor.Invoke(args.ToArray());

                        return obj;
                    }));
            }


        }

        private static IEnumerable<Type> GetImplementations(Type interfaceType, Assembly assembly)
        {

            var ass = assembly ?? Assembly.GetEntryAssembly();
            foreach (var ti in ass.DefinedTypes)
            {
                if (ti.ImplementedInterfaces.Contains(interfaceType))
                {
                    yield return ti.AsType();
                }
            }
        }

        private static IEnumerable<ConstructorInfo> GetConstructors(this Type type, Type baseParameterType)
        {
            return type.GetConstructors()
                .Where(ci => ci.GetParameters().Length == 1)
                .Where(ci => baseParameterType.IsAssignableFrom(ci.GetParameters().First().ParameterType));
        }

    }
}
