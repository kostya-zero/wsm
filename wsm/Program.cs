using CliFx;

namespace wsm
{
    public static class Program
    {
        public static async Task<int> Main() =>
            await new CliApplicationBuilder().AddCommandsFromThisAssembly()
                .Build()
                .RunAsync();
    }
}