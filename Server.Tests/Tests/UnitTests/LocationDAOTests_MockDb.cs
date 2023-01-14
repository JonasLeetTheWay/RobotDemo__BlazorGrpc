using BlazorGrpc.Server.Infrastructure;
using BlazorGrpc.Server.Settings;
using BlazorGrpc.Shared.DataGenerator;
using BlazorGrpc.Shared.Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using Xunit.Abstractions;

namespace Server.Tests.UnitTests;


static class MockHelper
{
    public static void VerifyFindSync<T>(this Mock<IMongoCollection<T>> mock, Times times)
    {
        mock.Verify(
        c => c.FindSync(It.IsAny<FilterDefinition<T>>(), It.IsAny<FindOptions<T, T>>(), It.IsAny<CancellationToken>()),
    times);
    }

    public static void VerifyUpdateOne<T>(this Mock<IMongoCollection<T>> mock, Times times)
    {
        mock.Verify(
        c => c.UpdateOne(It.IsAny<FilterDefinition<T>>(), It.IsAny<UpdateDefinition<T>>(), It.IsAny<UpdateOptions>(), It.IsAny<CancellationToken>()),
            times);
    }


    
}


public class DAOTests_MockDb<T>
{
    protected readonly ITestOutputHelper _logger;

    protected readonly Mock<IOptions<MongoDBSettings>> _mockOptions;
    protected readonly Mock<IMongoClient> _mockClient;
    protected readonly Mock<IMongoDatabase> _mockDB;
    protected readonly Mock<IMongoCollection<T>> _mockCollection;
    protected readonly Mock<IMongoDBContext> _mockContext;

    public DAOTests_MockDb(ITestOutputHelper logger)
    {
        _logger = logger;

        _mockOptions = new Mock<IOptions<MongoDBSettings>>();
        _mockDB = new Mock<IMongoDatabase>();
        _mockClient = new Mock<IMongoClient>();

        var settings = new MongoDBSettings()
        {
            ConnectionString = "mongodb://tes123 ",
            DatabaseName = "TestDB"
        };

        _mockOptions.Setup(s => s.Value).Returns(settings);
        _mockClient.Setup(c => c
        .GetDatabase(_mockOptions.Object.Value.DatabaseName, null))
            .Returns(_mockDB.Object);

        _mockCollection = new Mock<IMongoCollection<T>>();
        _mockContext = new Mock<IMongoDBContext>();

    }

}

public class LocationDAOTests_MockDb : DAOTests_MockDb<Location>
{
    private readonly Mock<ILogger<LocationDAO>> _mockLogger = new Mock<ILogger<LocationDAO>>();

    private Mock<LocationDAO> _mockLocationDAO;

    //protected void TestLog(object obj, string message = "", params object[] args)
    //{
    //    StringBuilder strBuilder = new();
    //    for (int i = 0; i < args.Length; i++)
    //    {
    //        strBuilder.Append(" " + args[i]);
    //    }
    //    _logger.WriteLine(message + obj?.ToString() + strBuilder.ToString());
    //}

    //protected void TestLog(IEnumerable<object> objs, string message = "", params object[] args)
    //{
    //    StringBuilder strBuilder = new();
    //    StringBuilder strBuilder2 = new();
    //    for (int i = 0; i < args.Length; i++)
    //    {
    //        strBuilder.Append(" " + args[i]);
    //    }
    //    foreach (var obj in objs)
    //    {
    //        _logger.WriteLine(message + obj?.ToString() + strBuilder.ToString());
    //    }

    //}

    protected void TestLog(string message = "", object? obj = null, params object[] args)
    {
        StringBuilder strBuilder = new();
        for (int i = 0; i < args.Length; i++)
        {
            strBuilder.Append(" " + args[i]);
        }
        _logger.WriteLine(message + obj?.ToString() + strBuilder.ToString());
    }

    protected void TestLog(string message = "", IEnumerable<object>? objs = null, params object[] args)
    {
        StringBuilder strBuilder = new();
        for (int i = 0; i < args.Length; i++)
        {
            strBuilder.Append(" " + args[i]);
        }
        foreach (var obj in objs)
        {
            _logger.WriteLine(message + obj?.ToString() + strBuilder.ToString());
        }

    }

    public LocationDAOTests_MockDb(ITestOutputHelper logger) : base(logger)
    {
    }

    [Fact]
    public void GetLocations_ShouldReturnAllLocations_WhenCalled()
    {
        // Arrange
        var _list = DomainDataGenerator.GenerateRandomLocations(3);
        TestLog("expected: ", _list);

        // Mock MoveNext
        Mock<IAsyncCursor<Location>> _cursor = new Mock<IAsyncCursor<Location>>();
        _cursor.Setup(_ => _.Current).Returns(_list);
        _cursor.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>())).Returns(true);

        // Mock FindSync
        _mockCollection.Setup(op => op.FindSync(It.IsAny<FilterDefinition<Location>>(),
        It.IsAny<FindOptions<Location, Location>>(),
        It.IsAny<CancellationToken>())).Returns(_cursor.Object);

        // Mock GetCollection
        _mockContext.Setup(c => c.GetCollection<Location>()).Returns(_mockCollection.Object);

        // MOCK instance
        _mockLocationDAO = new Mock<LocationDAO>(_mockContext.Object, _mockLogger.Object);
        var _locationDAO = _mockLocationDAO.Object;

        // REAL instance
        //_mockLocationDAO = new LocationDAO(_mockContext.Object, _mockLogger.Object); 

        // Act
        var result = _locationDAO.GetLocations();
        TestLog("res: ", res);

        // Assert 
        for (int i = 0; i < result.Count; i++)
        {
            Assert.NotNull(result[i]);
            Assert.Equal(_list[i].Id, result[i].Id);
            Assert.Equal(_list[i].Name, result[i].Name);
            Assert.Equal(_list[i].X, result[i].X);
            Assert.Equal(_list[i].Y, result[i].Y);
            Assert.Equal(_list[i].RobotIds, result[i].RobotIds);
            _logger.WriteLine(_list[i].ToString());
        }

        //Verify if GetLocations() is called once, which it innerly used FindSync() once
        _mockCollection.Verify(c => c.FindSync(It.IsAny<FilterDefinition<Location>>(),
            It.IsAny<FindOptions<Location>>(),
             It.IsAny<CancellationToken>()), Times.Once);

        // Verify if IAsyncCursor is moved listCount-1 times (2)
        _cursor.Verify(c => c.MoveNext(It.IsAny<CancellationToken>()), Times.Exactly(_list.Count - 1));
        _cursor.Verify(c => c.Current, Times.Once());
        _cursor.Verify(c => c.Dispose(), Times.Once());

        _mockCollection.VerifyNoOtherCalls();
        _cursor.VerifyNoOtherCalls();

    }

    [Fact]
    public void InsertLocation_ShouldInsertNewLocation_WhenLocationDoesNotExist() {
        //// Assumption: no existing location found in _mockCollection

        // Arrange
        var location = DomainDataGenerator.GenerateLocation(); 
      
        var expectedId = location.Id;

        // expect empty list (since we assume no existing location found in _mockCollection)
        var _list = new List<Location>();
        TestLog("generated Id: ", expectedId);
        
        _logger.WriteLine("DomainDataGenerator.GenerateLocation generated Id: " + expectedId);

        // Mock IAsyncCursor
        Mock<IAsyncCursor<Location>> _cursor = new Mock<IAsyncCursor<Location>>();
        _cursor.Setup(_ => _.Current).Returns(_list);
        _cursor.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>())).Returns(true);

        // Mock FindSync
        _mockCollection.Setup(op => op.FindSync(It.IsAny<FilterDefinition<Location>>(),
        It.IsAny<FindOptions<Location, Location>>(),
        It.IsAny<CancellationToken>())).Returns(_cursor.Object);


        // Mock InsertOne, since LocationDAO's InsertLocation is expected to be called
        _mockCollection.Setup(c => c.InsertOne(It.IsAny<Location>(), It.IsAny<InsertOneOptions>(),
        It.IsAny<CancellationToken>()));

        // Mock UpdateResult 
        Mock<UpdateResult> _update = new Mock<UpdateResult>();
        _update.Setup(u => u.IsAcknowledged).Returns(true);

        // Mock UpdateOne, since LocationDAO's UpdateLocation might be called (IF THERE IS ERROR)
        _mockCollection.Setup(c => c.UpdateOne(It.IsAny<FilterDefinition<Location>>(), It.IsAny<UpdateDefinition<Location>>(), It.IsAny<UpdateOptions>(), It.IsAny<CancellationToken>())).Returns(_update.Object);
            

        _mockContext.Setup(c => c.GetCollection<Location>()).Returns(_mockCollection.Object);

        var _locationDAO = new Mock<LocationDAO>(_mockContext.Object, _mockLogger.Object);

        // Act
        var result = _locationDAO.Object.InsertLocation(location);
        TestLog("locationDAO generated Id: ", res);
        

        // Assert
        Assert.Equal(expectedId, result);

        // VerifyFindSync if InsertOne is called once
        _mockCollection.Verify(
            c => c.InsertOne(It.IsAny<Location>(), It.IsAny<InsertOneOptions>(),It.IsAny<CancellationToken>()), 
            Times.Once);

        // VerifyFindSync if UpdateOne is never called (since we assume no existing location found in _mockCollection)
        _mockCollection.Verify(
            c => c.UpdateOne(It.IsAny<FilterDefinition<Location>>(), It.IsAny<UpdateDefinition<Location>>(), It.IsAny<UpdateOptions>(), It.IsAny<CancellationToken>()),
            Times.Never()
            );
    }

    [Fact]
    public void VerifyExistance_ShouldReturnNull_WhenLocationDoesNotExist() {
        //// Assumption: no existing location found in _mockCollection

        // Arrange
        var location = DomainDataGenerator.GenerateLocation();
        
        // expect empty list (since we assume no existing location found in _mockCollection)
        var _list = new List<Location>();

        // Mock IAsyncCursor
        Mock<IAsyncCursor<Location>> _cursor = new Mock<IAsyncCursor<Location>>();
        _cursor.Setup(_ => _.Current).Returns(_list);
        _cursor.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>())).Returns(true);

        // Mock FindSync
        _mockCollection.Setup(op => op.FindSync(It.IsAny<FilterDefinition<Location>>(),
        It.IsAny<FindOptions<Location, Location>>(),
        It.IsAny<CancellationToken>())).Returns(_cursor.Object);

        _mockContext.Setup(c => c.GetCollection<Location>()).Returns(_mockCollection.Object);

        var _locationDAO = new Mock<LocationDAO>(_mockContext.Object, _mockLogger.Object);

        // Act
        var result = _locationDAO.Object.VerifyExistance(location);
        TestLog("res: ", res);

        // Assert 
        Assert.Null(result);

        // VerifyFindSync if FindSync is called 3 times (every call of VerifyExistance uses 3 times)
        _mockCollection.VerifyFindSync(Times.Exactly(3));
       
    }

   
        TestLog("expected Location: ", expected);

        TestLog("locFilter: ", locFilter);
        TestLog("res: ", res);
    [Fact]
    public void VerifyExistance_ShouldReturnLocation_WhenLocationExistsWithSameId() { }
        TestLog("expected Location: ", expected);

        TestLog("locFilter: ", locFilter);
        TestLog("res: ", res);
    [Fact]
    public void VerifyExistance_ShouldReturnLocation_WhenLocationExistsWithSameCoordinates() { }
        TestLog("expected Location: ", expected);

        TestLog("locFilter: ", locFilter);
        TestLog("res: ", res);
    [Fact]
    public void VerifyExistance_ShouldThrowException_WhenLocationExistsWithSameNameAndDifferentCoordinates() { }
        TestLog("expected Location: ", expected);

        TestLog("locFilter: ", locFilter);

        TestLog("res: ", res);
    [Fact]
    public void FindLocations_ShouldReturnLocation_WhenFilterMatchesSingleLocation() { }
        TestLog("locFilter: ", locFilter);

        TestLog("_list item: ", _list);
        TestLog("res: ", result);
    [Fact]
    public void FindLocations_ShouldReturnMultipleLocations_WhenFilterMatchesMultipleLocations() { }
        TestLog("locFilter: ", locFilter);

        TestLog("_list item: ", _list);
        TestLog("res: ", result);
    [Fact]
    public void UpdateLocation_ShouldUpdateLocation_WhenFilterMatchesSingleLocation() { }

    [Fact]
    public void UpdateLocation_ShouldUpdateMultipleLocations_WhenFilterMatchesMultipleLocations() { }

    [Fact]
    public void DeleteLocation_ShouldDeleteLocation_WhenFilterMatchesSingleLocation() { }

    [Fact]
    public void DeleteLocation_ShouldDeleteMultipleLocations_WhenFilterMatchesMultipleLocations() { }


    






}

