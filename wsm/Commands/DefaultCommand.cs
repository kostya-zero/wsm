using Cocona;
using Cocona.Help;

namespace wsm.Commands;

public class DefaultCommand
{
    [PrimaryCommand]
    public void Default([FromService] ICoconaHelpMessageBuilder builder)
    {
        Console.WriteLine("\x1b[1mWindows Services Manager CLI\x1b[0m");
        Console.WriteLine(builder.BuildAndRenderForCurrentContext());
    }
}