using System;
using System.Collections.Generic;
using System.Text;

namespace solar.models
{
    public class AppSetting
    {
        public int id { get; set; }
        public string mode { get; set; }
        public string value { get; set; }
        public string alias { get; set; }
        public string type { get; set; }
    }
}
