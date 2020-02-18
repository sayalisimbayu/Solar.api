using System;
using System.Collections.Generic;
using System.Text;

namespace solar.models
{
    public class AppUser : Base
    {
        public int id { get; set; }
        public string username { get; set; }
        public string displayname { get; set; }
        public string password { get; set; }
        public string otp { get; set; }
        public bool isdeleted { get; set; }
        public string profileimg { get; set; }
        [NoBind]
        public AppPermission[] permissions { get; set; }
    }
}
