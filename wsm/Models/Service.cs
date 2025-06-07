using System.ServiceProcess;
using wsm.Exceptions;

namespace wsm.Models;

/// <summary>
/// Base class for service that utilizes the ServiceController class.
/// </summary>
public class Service(ServiceController serviceController)
{
    /// <summary>
    /// The ServiceController instance that interacts with the Windows service.
    /// </summary>
    private readonly ServiceController _serviceController = serviceController;

    /// <summary>
    /// The display name of the service.
    /// </summary>
    public string DisplayName => _serviceController.DisplayName;

    /// <summary>
    /// The real name (identifier) of the service.
    /// </summary>
    public string ServiceName => _serviceController.ServiceName;

    /// <summary>
    /// The description of the service, retrieved from the Windows registry.
    /// </summary>
    public string Description
    {
        get
        {
            string keyPath = $@"SYSTEM\CurrentControlSet\Services\{_serviceController.ServiceName}";
            using var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(keyPath);
            if (key == null) return "No description available.";
            string rawDescription = key.GetValue("Description")?.ToString() ?? "No description available.";
            if (!rawDescription.StartsWith('@')) return rawDescription;
            string? description = ResourceManager.ExpandResourceString(rawDescription);
            return description ?? rawDescription;
        }
        set => Description = value;
    }

    /// <summary>
    /// List of services that depend on this service by display name.
    /// </summary>
    public string[] Dependent => _serviceController.DependentServices.Select(t => t.DisplayName).ToArray();

    /// <summary>
    /// The path to the executable of the service, retrieved from the Windows registry.
    /// </summary>
    public string PathToExecutable
    {
        get
        {
            string keyPath = $@"SYSTEM\CurrentControlSet\Services\{_serviceController.ServiceName}";
            using var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(keyPath);
            if (key != null)
            {
                return key.GetValue("ImagePath")?.ToString() ?? "Error.";
            }

            return "Error.";
        }
        set => PathToExecutable = value;
    }

    /// <summary>
    /// List of services that this service depends on by display name.
    /// </summary>
    public string[] DependsOn => _serviceController.ServicesDependedOn.Select(t => t.DisplayName).ToArray();

    /// <summary>
    /// The user account under which the service runs, retrieved from the Windows registry.
    /// </summary>
    public string RunAs
    {
        get
        {
            string keyPath = $@"SYSTEM\CurrentControlSet\Services\{_serviceController.ServiceName}";
            using var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(keyPath);
            if (key != null)
            {
                return key.GetValue("ObjectName")?.ToString() ?? "Unknown";
            }

            return "Unknown";
        }
        set => RunAs = value;
    }

    /// <summary>
    /// Status of the service.
    /// </summary>
    public ServiceControllerStatus Status => _serviceController.Status;

    /// <summary>
    /// The start type of the service.
    /// </summary>
    public ServiceStartMode StartType => _serviceController.StartType;

    /// <summary>
    /// Start the service
    /// </summary>
    /// <param name="wait">If true, waits up to 15 seconds for the service to start. Default is true</param>
    /// <exception cref="ServiceOperationException">An error occurred while accessing Windows API.</exception>
    /// <exception cref="PermissionsException">Not enough permissions to do this.</exception>
    /// <exception cref="UnexpectedException">An unexpected error.</exception>
    public void Start(bool wait)
    {
        if (_serviceController.Status != ServiceControllerStatus.Running)
        {
            try
            {
                _serviceController.Start();
                if (wait)
                {
                    _serviceController.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(15));
                }
            }
            catch (System.ComponentModel.Win32Exception)
            {
                throw new ServiceOperationException();
            }
            catch (InvalidOperationException)
            {
                throw new PermissionsException();
            }
            catch (Exception ex)
            {
                throw new UnexpectedException($"Unexpected error: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Refresh the service current state.
    /// </summary>
    public void Refresh()
    {
        _serviceController.Refresh();
    }

    /// <summary>
    /// Resume a paused service.
    /// </summary>
    /// <param name="wait">If true, waits up to 15 seconds for the service to resume. Default is true</param>
    /// <exception cref="ServiceOperationException">An error occured while accessing Windows API.</exception>
    /// <exception cref="PermissionsException">Not enough permissions to do this.</exception>
    /// <exception cref="UnexpectedException">An unexpected error.</exception>
    public void Resume(bool wait)
    {
        if (_serviceController.Status == ServiceControllerStatus.Paused)
        {
            try
            {
                _serviceController.Continue();
                if (wait)
                {
                    _serviceController.WaitForStatus(ServiceControllerStatus.Running,
                        TimeSpan.FromSeconds(15)); // Aligning timeout with Start method
                }
            }
            catch (System.ComponentModel.Win32Exception)
            {
                throw new ServiceOperationException();
            }
            catch (InvalidOperationException)
            {
                throw new PermissionsException();
            }
            catch (Exception ex)
            {
                throw new UnexpectedException($"Unexpected error: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Pause a running service.
    /// </summary>
    /// <param name="wait">If true, waits up to 15 seconds for the service to pause. Default is true</param>
    /// <exception cref="ServiceOperationException">An error occured while accessing Windows API.</exception>
    /// <exception cref="PermissionsException">Not enough permissions to do this.</exception>
    /// <exception cref="UnexpectedException">An unexpected error.</exception>
    public void Pause(bool wait)
    {
        if (_serviceController.Status == ServiceControllerStatus.Running)
        {
            try
            {
                _serviceController.Pause();
                if (wait)
                {
                    _serviceController.WaitForStatus(ServiceControllerStatus.Paused,
                        TimeSpan.FromSeconds(15)); // Aligning timeout with Start method
                }
            }
            catch (System.ComponentModel.Win32Exception)
            {
                throw new ServiceOperationException();
            }
            catch (InvalidOperationException)
            {
                throw new PermissionsException();
            }
            catch (Exception ex)
            {
                throw new UnexpectedException($"Unexpected error: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Restart the service
    /// </summary>
    /// <param name="wait">If true, waits up to 15 seconds for the service to stop before starting it again. Default is true</param>
    /// <exception cref="ServiceOperationException">An error occured while accessing Windows API.</exception>
    /// <exception cref="PermissionsException">Not enough permissions to do this.</exception>
    /// <exception cref="UnexpectedException">An unexpected error.</exception>
    public void Restart(bool wait)
    {
        if (_serviceController.Status == ServiceControllerStatus.Running)
        {
            Stop(wait);
        }

        Start(wait);
    }

    /// <summary>
    /// Stop the service
    /// </summary>
    /// <param name="wait">If true, waits up to 15 seconds for the service to stop. Default is true.</param>
    /// <exception cref="ServiceOperationException">An error occured while accessing Windows API.</exception>
    /// <exception cref="PermissionsException">Not enough permissions to do this.</exception>
    /// <exception cref="UnexpectedException">An unexpected error.</exception>
    public void Stop(bool wait)
    {
        if (_serviceController.Status != ServiceControllerStatus.Stopped)
        {
            try
            {
                _serviceController.Stop();
                if (wait)
                {
                    _serviceController.WaitForStatus(ServiceControllerStatus.Stopped,
                        TimeSpan.FromSeconds(15)); // Aligning timeout with Start method
                }
            }
            catch (System.ComponentModel.Win32Exception)
            {
                throw new ServiceOperationException();
            }
            catch (InvalidOperationException)
            {
                throw new PermissionsException();
            }
            catch (Exception ex)
            {
                throw new UnexpectedException($"Unexpected error: {ex.Message}");
            }
        }
    }
}