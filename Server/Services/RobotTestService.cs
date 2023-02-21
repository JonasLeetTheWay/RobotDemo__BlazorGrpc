using BlazorGrpc.Server.Infrastructure;

namespace BlazorGrpc.Server.Services;

public class RobotTestService : SmartRCService.RobotTest.Protos.RobotTestService.RobotTestServiceBase
{
    public LocationService(ILocationDAO locationDAO, ILogger? logger = null)
    {
        this.locationDAO = locationDAO;
        _logger = logger;
    }
}
