using System.Reflection;

using Paddi.DemoUsages.Core;

var assembly = Assembly.GetExecutingAssembly();
var runners = assembly.GetTypes().Where(c => c.IsAssignableTo(typeof(IRunnableService)));
// execute them in serial
foreach (var runner in runners)
{
    var instance = Activator.CreateInstance(runner) as IRunnableService;
    await instance!.RunAsync();
}
