using System;
using System.Collections.Generic;
using System.Text;

namespace solar.models
{
    public class AppUserInfo
    {
        public int ID { get; set; }
        public int USID { get; set; }
        [NoBind]
        public string DISPLAYNAME { get; set; }
        [NoBind]
        public string EMAIL { get; set; }
        public string FIRSTNAME { get; set; }
        public string LASTNAME { get; set; }
        public bool GENDER { get; set; }
        public string MOBILE { get; set; }
        public string SOCIAL { get; set; }
        public DateTime BIRTHDATE { get; set; }
        public string ADDRESSLINE1 { get; set; }
        public string ADDRESSLINE2 { get; set; }
        public string CITY { get; set; }
        public string USTATE { get; set; }
        public string COUNTRYCODE { get; set; }
    }
}
