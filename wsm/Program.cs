using Cocona;
using wsm.Commands;

var app = CoconaApp.Create();

app.Environment.ApplicationName = "Windows Service Manager CLI";

app.AddCommands<DefaultCommand>();
app.AddCommands<StartCommand>();
app.AddCommands<StopCommand>();
app.AddCommands<ListCommand>();
app.AddCommands<PauseCommand>();
app.AddCommands<ResumeCommand>(); 
app.AddCommands<RestartCommand>();

app.Run();