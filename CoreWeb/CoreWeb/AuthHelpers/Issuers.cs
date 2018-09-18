using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWeb.AuthHelpers
{
    public class Issuers
    {
        public static string ValidIssuer = "https://localhost:44350";
        public static string Issuer = "https://localhost:44350";

        public static string ValidAudience = "https://localhost:44350";
        public static string Audience = "https://localhost:44350";


        public static string GetKey()
        {
            // TODO: Store this somewhere more secure.
            return "1k1n254pojcqkn2]kn";
        }

    }
}
