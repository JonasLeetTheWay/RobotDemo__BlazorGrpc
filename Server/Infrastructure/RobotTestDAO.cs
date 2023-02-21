using BlazorGrpc.Shared.Domain;

namespace BlazorGrpc.Server.Infrastructure;

public class RobotTestDAO : BaseDAO<RobotTestData>
{
    public RobotTestDAO(IMongoDBContext context, ILogger<RobotTestData>? logger) : base(context, logger)
    {
    }
}


