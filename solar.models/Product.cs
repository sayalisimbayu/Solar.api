using System;
using System.Collections.Generic;

namespace solar.models
{
    public class Products: Base
    {
        public int id { get; set; }
        public string name { get; set; }
        public string subheader { get; set; }
        public decimal price { get; set; }
        public int gst { get; set; }
        public bool isdeleted { get; set; }
        [NoBind]
        public ProductCategory[] categories { get; set; }
    }
}
