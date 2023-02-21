using BlazorGrpc.Shared.Domain;

namespace BlazorGrpc.Server.Infrastructure;

public class RobotTestDAO : BaseDAO<RobotTestData>
{
    public RobotTestDAO(IMongoDBContext context, ILogger<RobotTestData>? logger) : base(context, logger)
    {
    }
}

/*
public class BatteryTest : GeneralTestData
{
}
public class LidarTest : GeneralTestData
{
}

public class IOTest : GeneralTestData
{
}

public class IMUTest : GeneralTestData
{
}

public class FrontCameraTest : GeneralTestData
{
}
public class RearCameraTest : GeneralTestData
{
}

public class WheelMotorTest : GeneralTestDataWithAngle
{
}

public class LEDTest : GeneralTestData
{
}

public class BuzzerTest : GeneralTestData
{
}

public class EmergencyStopTest : GeneralTestData
{
}
 */

