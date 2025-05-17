using Cocona;
using wsm.Commands;

var app = CoconaApp.Create();

app.Environment.ApplicationName = "Windows Service Manager CLI";

app.AddCommands<DefaultCommand>();
app.AddCommands<StartCommand>();
app.AddCommands<ListCommand>();

app.Run();