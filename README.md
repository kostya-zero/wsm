> [!WARNING]
> This project has been discontinued. I’ve realized that using PowerShell scripting is a more effective and native option than writing another program, as PowerShell is pre-installed on most Windows installations. Also, I want to focus primarily on Rust rather than other programming languages.

# Windows Services Manager

**WSM** is a command-line tool for managing Windows services. 
It allows you to start, stop, restart, and check the status of services on your Windows machine. 
It also provides a way to list all services and filter them by their status.

## Installation

Get the archive with version for your architecture from the [releases page](https://github.com/kostya-zero/wsm/releases) and extract it to a directory in your PATH.

## Usage
You can perform all basic operations with services:
```powershell
wsm start <service_name>
wsm stop <service_name>
wsm restart <service_name>
wsm resume <service_name>
wsm pause <service_name>
```

Get the list of all services and sort them:
```powershell
# Sort by name
wsm list

# Custom sort order (e.g. by status)
wsm list --sort status
```

Also get information about a specific service:
```powershell
wsm info <service_name>
```

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
