using Microsoft.Extensions.DependencyInjection;

namespace Paddi.DemoUsages.WpfDemo;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        Services = ConfigureServices();
    }

    public IServiceProvider Services;

    public new static App Current => (App)Application.Current;

    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();


        return services.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
    }
}
