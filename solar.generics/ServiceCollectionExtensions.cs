using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace solar.generics
{
    public static class ServiceCollectionExtensions
    {
        public static void AddScopedDynamic<TInterface>(this IServiceCollection services, IEnumerable<Type> types)
        {
            services.AddScoped<Func<string, TInterface>>(serviceProvider => tenant =>
            {
                var itype = typeof(TInterface);
                var type = types
                    .Where(p => itype.IsAssignableFrom(p) && (String.IsNullOrEmpty(tenant) ? true : ((TypeInfo)p).ImplementedInterfaces.Count(y => y.Name == tenant) > 0)
                    ).LastOrDefault();
                if (null == type)
                    throw new KeyNotFoundException("No instance found for the given tenant.");

                return (TInterface)serviceProvider.GetService(type);
            });
        }
    }
}
