namespace Microservices.Services.EmailAPI.Utility
{
    public class SD
    {
        public static string _serviceBusConnectionString;
        public static string _emailCartQueue;
        public static string _emailregisterUserQueue;
        public static string? _emailOrderCreatedTopicName { get; set; }
        public static string? _emailOrderCreatedSubscription { get; set; }
    }
}
