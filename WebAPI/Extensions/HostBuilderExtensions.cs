using Autofac.Extensions.DependencyInjection;
using Autofac;
using Business.DependencyResolvers.Autofac;

namespace WebAPI.Extensions
{
    public static class HostBuilderExtensions
    {
        public static void ConfigureAutofacProviderFactory(this IHostBuilder host)
        {
            host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterModule(new BusinessModule());
                });
        }
    }
}
