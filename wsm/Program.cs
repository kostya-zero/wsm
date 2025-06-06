﻿using Cocona;
using wsm.Commands;

var app = CoconaApp.Create();

app.AddCommands<DefaultCommand>();
app.AddCommands<StartCommand>();
app.AddCommands<StopCommand>();
app.AddCommands<ListCommand>();
app.AddCommands<InfoCommand>();
app.AddCommands<PauseCommand>();
app.AddCommands<ResumeCommand>(); 
app.AddCommands<RestartCommand>();

app.Run();