namespace BlazorGrpc.Shared.Domain;

public class GeneralTestDataWithAngle : GeneralTestData
{
    protected float? angle { get; private set; }
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