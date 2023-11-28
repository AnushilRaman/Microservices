namespace Microservices.Web.Utility
{
    public class SD
    {
        public static string CouponApiBase { get; set; }
        public static string AuthApiBase { get; set; }
        public static string ProductApiBase { get; set; }
        public static string CartApiBase { get; set; }

        public const string RoleAdmin = "ADMIN";
        public const string RoleCustomer = "CUSTOMER";
        public const string TokenCookies = "JWTToken";




        public enum ApiName
        {
            api,
            CartApi,
            CouponApi,
            ProductApi,
            AuthAPI
        }

        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
