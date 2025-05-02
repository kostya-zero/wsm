using System.ServiceProcess;
using System.Security.Principal;
using wsm.Exceptions;

namespace wsm.Models;

/// <summary>
/// Base class for service that utilizes the ServiceController class.
/// </summary>
public class Service
{
    /// <summary>
    /// The ServiceController instance that interacts with the Windows service.
    /// </summary>
    private readonly ServiceController _serviceController;

    /// <summary>
    /// The display name of the service.
    /// </summary>
    public string DisplayName => _serviceController.DisplayName;

    /// <summary>
    /// The real name (identifier) of the service.
    /// </summary>
    public string ServiceName => _serviceController.ServiceName;

    /// <summary>
    /// Status of the service.
    /// </summary>
    public ServiceControllerStatus Status => _serviceController.Status;

    public Service(ServiceController serviceController)
    {
        _serviceController = serviceController;
    }

    /// <summary>
    /// Start the service
    /// </summary>
    /// <param name="wait">If true, waits up to 15 seconds for the service to start. Default is true</param>
    /// <exception cref="ServiceOperationException">An error occured while accessing Windows API.</exception>
    /// <exception cref="PermissionsException">Not enough permissions to do this.</exception>
    /// <exception cref="UnexpectedException">An unexpected error.</exception>
    public void Start(bool wait = true)
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
            catch (System.ComponentModel.Win32Exception ex)
            {
                throw new ServiceOperationException();
            }
            catch (InvalidOperationException ex)
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
    public void Resume(bool wait = true)
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
            catch (System.ComponentModel.Win32Exception ex)
            {
                throw new ServiceOperationException();
            }
            catch (InvalidOperationException ex)
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
    public void Pause(bool wait = true)
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
            catch (System.ComponentModel.Win32Exception ex)
            {
                throw new ServiceOperationException();
            }
            catch (InvalidOperationException ex)
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
    public void Restart(bool wait = true)
    {
        if (_serviceController.Status == ServiceControllerStatus.Running)
        {
            Stop(wait);
        }
        Start(wait);
    }

    /// <summary>
    /// Restart the service
    /// </summary>
    /// <param name="wait">If true, waits up to 15 seconds for the service to stop. Default is true.</param>
    /// <exception cref="ServiceOperationException">An error occured while accessing Windows API.</exception>
    /// <exception cref="PermissionsException">Not enough permissions to do this.</exception>
    /// <exception cref="UnexpectedException">An unexpected error.</exception>
    public void Stop(bool wait = true)
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
            catch (System.ComponentModel.Win32Exception ex)
            {
                throw new ServiceOperationException();
            }
            catch (InvalidOperationException ex)
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