namespace Orc;

using Catel.Services;
using Catel.ThirdPartyNotices;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Core module which allows the registration of default services in the service collection.
/// </summary>
public static class OrcPlotModule
{
    public static IServiceCollection AddOrcPlot(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ILanguageSource>(new LanguageResourceSource("Orc.Plot", "Orc.Plot.Properties", "Resources"));

        serviceCollection.AddSingleton<IThirdPartyNotice>((x) => new LibraryThirdPartyNotice("Orc.Plot", "https://github.com/wildgums/orc.plot"));
        serviceCollection.AddSingleton<IThirdPartyNotice>((x) => new ResourceBasedThirdPartyNotice("OxyPlot", "https://github.com/oxyplot/oxyplot/", "Orc.Plot", "Orc.Plot", "Resources.ThirdPartyNotices.oxyplot.txt"));

        return serviceCollection;
    }
}
