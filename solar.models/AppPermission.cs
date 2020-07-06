using System;
using System.Collections.Generic;
using System.Text;

namespace solar.models
{
    public class AppPermission : Base
    {
        public int id { get; set; }
        public int appuserid { get; set; }
        public int appsettingid { get; set; }
        public int permission { get; set; }
        [NoBind]
        public string mode { get; set; }
        [NoBind]
        public string alias { get; set; }
        [NoBind]
        public string value { get; set; }
        [NoBind]
        public string description { get; set; }
    }
}
