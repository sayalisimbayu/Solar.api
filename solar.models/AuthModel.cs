using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace solar.api
{
    public class AuthModel
    {
        public string email { get; set; }
        public string password { get; set; }
    }

    public class ResetModel: AuthModel
    {
        public string otp { get; set; }
    }
}
