// ServiceRepository.cs
using System.ServiceProcess;
using wsm.Models;

namespace wsm.Repositories;

public static class ServiceRepository
{
    public static IEnumerable<Service> GetAllServices()
    {
        return ServiceController.GetServices().Select(sc => new Service(sc));
    }
    
    public static Service? GetServiceByName(string name)
    {
        var service = ServiceController.GetServices()
            .FirstOrDefault(s => s.ServiceName.Equals(name, StringComparison.OrdinalIgnoreCase) || 
                                  s.DisplayName.Equals(name, StringComparison.OrdinalIgnoreCase));
                                  
        return service != null ? new Service(service) : null;
    }
}