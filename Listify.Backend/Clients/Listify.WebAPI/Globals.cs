using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Listify.WebAPI
{
    public static class Globals
    {
        public const string DEV_CONNECTION_STRING = "Server=(LocalDb)\\MSSQLLocalDB;Database=Listify;Trusted_Connection=true;MultipleActiveResultSets=true";
        public const string ANGULAR_WEBAPP_URL = "http://localhost:4200";
        public const string IDENTITY_SERVER_AUTHORITY_URL = "http://localhost:5000";
    }
}
