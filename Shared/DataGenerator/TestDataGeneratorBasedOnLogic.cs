using BlazorGrpc.Shared.Domain;

namespace BlazorGrpc.Shared.DataGenerator;

public static class TestDataGeneratorBasedOnLogic
{
    public static class ForIAsyncCursor
    {
        /// <summary>
        /// Checks if the location filter (locFilter) matches the current location (loc) in any of 3 ways: 
        /// <br/><br/>
        /// <br/>1. by Id,
        /// <br/>2. by Name,
        /// <br/>3. by X and Y coordinates.
        /// <br/><br/>
        /// If any of these conditions are true, it creates a list containing the stored location. Otherwise, it creates an empty list.
        /// </summary>
        /// <param name="locFilter"></param>
        /// <param name="expected"></param>
        /// <returns>List of Location</returns>
        public static List<Location> MakeLocationList(Location locFilter, Location expected)
        {
            List<Location> list = new();
            if (locFilter.Id == expected.Id || locFilter.Name == expected.Name || ((locFilter.X == expected.X) && (locFilter.Y == expected.Y)))
            {
                list.Add(expected);
            }
            return list;
        }
        /// <summary>
        /// Checks if the location filter (locFilter) matches the current location (loc) in any of 3 ways: 
        /// <br/><br/>
        /// <br/>1. by Id,
        /// <br/>2. by Name,
        /// <br/>3. by X and Y coordinates.
        /// <br/><br/>
        /// If any of these conditions are true, it creates a list containing the stored location. Otherwise, it creates an empty list.
        /// </summary>
        /// <param name="locFilter"></param>
        /// <param name="expected"></param>
        /// <returns>List of Location</returns>
        public static List<Location> MakeLocationList(Location locFilter, List<Location> expecteds)
        {
            List<Location> list = new();
            foreach (var expected in expecteds)
            {
                if (locFilter.Id == expected.Id || locFilter.Name == expected.Name || ((locFilter.X == expected.X) && (locFilter.Y == expected.Y)))
                {
                    list.Add(expected);
                }
            }
            return list;
        }
    }
}