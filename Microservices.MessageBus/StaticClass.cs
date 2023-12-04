namespace Microservices.MessageBus
{
    public class StaticClass
    {
        public static string? MessageBusConnectionString { get; set; }
        public static string? EmailShoppingCart { get; set; }
        public static string? EmailregisterUserCart { get; set; }


        public const string Status_Pending = "Pending";
        public const string Status_Approved = "Approved";
        public const string Status_ReadyForPickup = "ReadyForPickup";
        public const string Status_Completed = "Copleted";
        public const string Status_Refunded = "Refunded";
        public const string Status_Cancelled = "Cancelled";
        
        
        public const string RoleAdmin = "ADMIN";
        public const string RoleCustomer = "CUSTOMER";
    }
}
