using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bdNetCoreAPI
{
    public static class ApiConstants
    {
        public const string Title = "My API";
        public const string ApiVersion = "1.0";
        public const string ApiDescription = "A Simple API";
        public const string ApiTermsOfService = "";
        public const bool ValidateIssuer = true;
        public const bool ValidateAudience = true;
        public const bool ValidateLifetime = true;
        public const bool ValidateIssuerSigningKey = true;
        public const string ValidIssuer = "yourdomain.com";
        public const string ValidAudience = "yourdomain.com";
    }
}
