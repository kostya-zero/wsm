using Cocona;
using Cocona.Help;

namespace wsm.Commands;

public class DefaultCommand
{
    [PrimaryCommand]
    public void Default([FromService] ICoconaHelpMessageBuilder builder)
    {
        Console.WriteLine("Windows Service Manager CLI");
        Console.WriteLine(builder.BuildAndRenderForCurrentContext());
    }
}