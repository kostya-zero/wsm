using Cocona;
using Cocona.Help;

namespace wsm.Commands;

public abstract class DefaultCommand
{
    [PrimaryCommand]
    public void Default([FromService] ICoconaHelpMessageBuilder builder)
    {
        Console.WriteLine("\e[1mWindows Services Manager CLI\e[0m");
        Console.WriteLine(builder.BuildAndRenderForCurrentContext());
    }
}