﻿using System;
using System.Collections.Generic;
using System.Text;

namespace solar.models
{
    public class AppUserAddOnConfig
    {
        public int ID { get; set; }
        public int USID { get; set; }
        public string LANGUAGE { get; set; }
        public string TIMEZONE { get; set; }
        public string DATEFORMAT { get; set; }
        public string LUCIDNOTIFICATION { get; set; }
        public string THEME { get; set; }
        public string SKIN { get; set; }
        public string SOCIALLIST { get; set; }
    }

    public class AppUserTheme {
        public int USID { get; set; }
        public string THEME { get; set; }
        public string SKIN { get; set; }
    }
}
