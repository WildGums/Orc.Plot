namespace Orc.Plot.Tests
{
    using Catel;
    using Microsoft.Extensions.DependencyInjection;
    using Orc.Plot;

    internal static class ServiceCollectionHelper
    {
        public static IServiceCollection CreateServiceCollection()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging();
            serviceCollection.AddCatelCore();
            serviceCollection.AddOrcPlot();

            return serviceCollection;
        }
    }
}
