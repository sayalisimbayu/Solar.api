using System;
using System.Collections.Generic;
using System.Text;

namespace solar.models
{
    public class Category : Base
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool isdeleted { get; set; }
        [NoBind]
        public int productcount { get; set; }
        [NoBind]
        public ProductCategory[] products { get; set; }
    }
}
