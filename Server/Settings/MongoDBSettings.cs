namespace BlazorGrpc.Server.Settings
{
    public class MongoDBSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string CollectionName_Locations { get; set; } = null!;
        public string CollectionName_Robots { get; set; } = null!;
    }
}