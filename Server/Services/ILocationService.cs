using BlazorGrpc.Shared;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace BlazorGrpc.Server.Services
{
    public interface ILocationService
    {
        Task<LocationId> AddLocation(AddLocationRequest request, ServerCallContext context);
        Task<Empty> AddRobotToLocation(LocationIdAndRobotId request, ServerCallContext context);
        Task<Empty> DeleteLocation(LocationId request, ServerCallContext context);
        Task<LocationResponse> GetLocationByCoordinate(Coordinate request, ServerCallContext context);
        Task<LocationResponse> GetLocationById(LocationId locationId, ServerCallContext context);
        Task<LocationResponse> GetLocationByName(Name request, ServerCallContext context);
        Task<LocationsResponse> GetLocationByRobotId(RobotId robotId, ServerCallContext context);
        Task<LocationsResponse> GetLocations(Empty request, ServerCallContext context);
        Task<Empty> RemoveRobotFromLocation(LocationIdAndRobotId request, ServerCallContext context);
        Task<Empty> UpdateLocation(LocationIdAndUpdate request, ServerCallContext context);
    }
}