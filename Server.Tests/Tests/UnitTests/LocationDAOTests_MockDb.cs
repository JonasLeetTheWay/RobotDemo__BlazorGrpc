﻿using BlazorGrpc.Server.Infrastructure;
using BlazorGrpc.Server.Settings;
using BlazorGrpc.Shared.DataGenerator;
using BlazorGrpc.Shared.Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using Xunit.Abstractions;

namespace Server.Tests.UnitTests;

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

    public LocationDAOTests_MockDb(ITestOutputHelper logger) : base(logger)
    {
    }

    [Fact]
    public void GetLocations_ShouldReturnAllLocations_WhenCalled()
    {
        // Arrange
        var _list = DomainDataGenerator.GenerateRandomLocations(3);

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
    public void InsertLocation_ShouldInsertNewLocation_WhenLocationDoesNotExist() { }

    [Fact]
    public void VerifyExistance_ShouldReturnNull_WhenLocationDoesNotExist() { }

    [Fact]
    public void VerifyExistance_ShouldReturnLocation_WhenLocationExistsWithSameId() { }

    [Fact]
    public void VerifyExistance_ShouldReturnLocation_WhenLocationExistsWithSameCoordinates() { }

    [Fact]
    public void VerifyExistance_ShouldThrowException_WhenLocationExistsWithSameNameAndDifferentCoordinates() { }


    [Fact]
    public void FindLocations_ShouldReturnLocation_WhenFilterMatchesSingleLocation() { }

    [Fact]
    public void FindLocations_ShouldReturnMultipleLocations_WhenFilterMatchesMultipleLocations() { }

    [Fact]
    public void UpdateLocation_ShouldUpdateLocation_WhenFilterMatchesSingleLocation() { }

    [Fact]
    public void UpdateLocation_ShouldUpdateMultipleLocations_WhenFilterMatchesMultipleLocations() { }

    [Fact]
    public void DeleteLocation_ShouldDeleteLocation_WhenFilterMatchesSingleLocation() { }

    [Fact]
    public void DeleteLocation_ShouldDeleteMultipleLocations_WhenFilterMatchesMultipleLocations() { }


    






}

