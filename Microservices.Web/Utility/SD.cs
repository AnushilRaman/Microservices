﻿namespace Microservices.Web.Utility
{
    public class SD
    {
        public static string CouponApiBase {  get; set; }
        public static string AuthApiBase {  get; set; }

        public enum ApiName
        {
            api,
            CouponApi,
            AuthApi
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
