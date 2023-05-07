using System.Reflection;

try
{
    var assembly = Assembly.GetExecutingAssembly();
    var runners = assembly.GetTypes().Where(c => c.IsAssignableTo(typeof(IRunnableService)));
    // execute them in serial
    foreach (var runner in runners)
    {
        var instance = Activator.CreateInstance(runner) as IRunnableService;
        if (instance!.Skip)
        {
            continue;
        }
        await instance!.RunAsync();
    }
}
catch (Exception)
{

}
Console.WriteLine("Press any key to continue...");
Console.ReadKey();
