// Service.cs
using System.Diagnostics.Contracts;
using System.ServiceProcess;

namespace wsm.Models;

public class Service
{
    private readonly ServiceController _serviceController;

    public string DisplayName => _serviceController.DisplayName;
    public string ServiceName => _serviceController.ServiceName;
    public ServiceControllerStatus Status => _serviceController.Status;

    public Service(ServiceController serviceController)
    {
        _serviceController = serviceController;
    }

    public void Start()
    {
        if (_serviceController.Status != ServiceControllerStatus.Running)
        {
            try
            {
                _serviceController.Start();
                _serviceController.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(15));
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                Console.WriteLine($"Error starting service: {ex.Message}");
                Console.WriteLine("Make sure you have the necessary permissions to start this service.");
                return;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Error starting service: {ex.Message}");
                Console.WriteLine("Make sure you have the necessary permissions to start this service.");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return;
            }
        }
    }

    public void Refresh()
    {
        // Refresh the service status
        _serviceController.Refresh();
    }

    public void Stop()
    {
        if (_serviceController.Status != ServiceControllerStatus.Stopped)
        {
            try
            {
                _serviceController.Stop();
                _serviceController.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(15));
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                Console.WriteLine($"Error stopping service: {ex.Message}");
                Console.WriteLine("Make sure you have the necessary permissions to stop this service.");
                return;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Error stopping service: {ex.Message}");
                Console.WriteLine("Make sure you have the necessary permissions to stop this service.");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return;
            }
        }
    } 
    
    // Add more methods like Stop(), Restart(), etc.
}