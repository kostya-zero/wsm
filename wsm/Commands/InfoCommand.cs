using Cocona;
using wsm.Models;
using wsm.Repositories;

namespace wsm.Commands;

public class InfoCommand
{
    public static void WriteWrapped(string text, int width = 0)
    {
        if (width == 0)
            width = Console.WindowWidth;

        var words = text.Split(' ');
        var line = "";

        foreach (var word in words)
        {
            // +1 is for space
            if ((line.Length + word.Length + 1) > width)
            {
                Console.WriteLine(line.TrimEnd());
                line = "";
            }
            line += word + " ";
        }
        if (line.Length > 0)
            Console.WriteLine(line.TrimEnd());
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
        if (service.DependsOn.Length > 0)
        {
            Console.WriteLine("\x1b[1mDepends On:\x1b[0m");
            foreach (var dependency in service.DependsOn)
            {
                Console.WriteLine("  - " + dependency);
            }
        }
        
    }
}