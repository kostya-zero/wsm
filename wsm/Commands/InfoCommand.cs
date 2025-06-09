using System.Text;
using Cocona;
using wsm.Models;
using wsm.Repositories;

namespace wsm.Commands;

public abstract class InfoCommand
{
    public static void WriteWrapped(string text, int width)
    {
        var words = text.Split(' ');
        StringBuilder bld = new StringBuilder();

        foreach (var word in words)
        {
            // +1 is for space
            if (bld.Length + word.Length + 1 > width)
            {
                Console.WriteLine(bld.ToString().TrimEnd());
                bld.Clear();
            }

            bld.Append(word);
            bld.Append(' ');
        }
        if (bld.Length > 0)
            Console.WriteLine(bld.ToString().TrimEnd());
    }
    
    [Command("info", Description = "Display information about the application.")]
    public void Info([Argument(Description = "The name of the service.")] string name)
    {
        Service? service = ServiceRepository.GetServiceByName(name);
        if (service == null)
        {
            Console.WriteLine("Service not found.");
            return;
        }
        
        Console.WriteLine($"\x1b[1m{service.DisplayName} ({service.ServiceName})\x1b[0m");
        WriteWrapped(service.Description, 80);
        
        Console.WriteLine("\n\x1b[1mStatus:\x1b[0m " + service.Status);
        Console.WriteLine("\x1b[1mStartup Type:\x1b[0m " + service.StartType);
        Console.WriteLine("\x1b[1mRun As:\x1b[0m " + service.RunAs);
        
        Console.WriteLine("\x1b[1mPath to Executable:\x1b[0m ");
        Console.WriteLine("  " + service.PathToExecutable);
        if (service.DependsOn.Length <= 0) return;
        Console.WriteLine("\x1b[1mDependencies:\x1b[0m");
        foreach (var dependency in service.DependsOn)
        {
            Console.WriteLine("  - " + dependency);
        }

    }
}