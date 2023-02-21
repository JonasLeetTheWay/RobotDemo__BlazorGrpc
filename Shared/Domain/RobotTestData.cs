using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BlazorGrpc.Shared.Domain;

public class RobotTestData : Entity
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("test_name")]
    public string? TestName { get; set; }

    [BsonElement("datetime")]
    public DateTime? Datetime { get; set; }

    public BatteryTest? BatteryTest { get; set; }
    public LidarTest? LidarTest { get; set; }
    public IOTest? IoTest { get; set; }
    public IMUTest? ImuTest { get; set; }
    public FrontCameraTest? FrontCamTest { get; set; }
    public RearCameraTest? RearCamTest { get; set; }
    public WheelMotorTest? WheelMotorTest { get; set; }
    public LEDTest? LedTest { get; set; }
    public BuzzerTest? BuzzerTest { get; set; }
    public EmergencyStopTest? EmergencyStopTest { get; set; }

}



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

/*
	// Automatic
	rpc PostBatteryTest(SimpleRequest) returns (SimpleResponse){};
	rpc PostLidarTest(SimpleRequest) returns (SimpleResponse){};
	rpc PostIOTest(SimpleRequest) returns (SimpleResponse){}; // 根據延伸模組的功能就 只給下功能指令
	rpc PostIMUTest(SimpleRequest) returns (SimpleResponse){}; // 直接while-loop print資料
	rpc PostFrontCameraTest(SimpleRequest) returns (SimpleResponse){}; // 直接while-loop print資料
	rpc PostRearCameraTest(SimpleRequest) returns (SimpleResponse){}; // 直接while-loop print資料
	rpc PostWheelMotorTest(SimpleRequest) returns (SimpleResponse){}; // 直接while-loop print資料
	rpc PostWheelMotorRelPositionRotateTest(SimpleRequestWithAngle) returns (SimpleResponseWithAngle){};
	rpc PostWheelMotorConstantRotateTest(SimpleRequest) returns (SimpleResponse){}; // 寫死持續運轉時間



	// Manual
	rpc PostLEDTest(SimpleRequest) returns (SimpleResponse){}; // 開燈功能而已
	rpc PostBuzzerTest(SimpleRequest) returns (SimpleResponse){}; // 寫死頻率
	rpc PostEmergencyStopTest(SimpleRequest) returns (SimpleResponse){}; // 死人狀態
 */