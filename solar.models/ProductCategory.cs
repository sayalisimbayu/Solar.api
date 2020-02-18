using System;
using System.Collections.Generic;
using System.Text;

namespace solar.models
{
    public class ProductCategory : Base
    {
        public int id { get; set; }
        public int productid { get; set; }
        public int categoryid { get; set; }
        public bool isprimary { get; set; }
        public bool isdeleted { get; set; }
    }
}
