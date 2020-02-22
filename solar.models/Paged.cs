using System;
using System.Collections.Generic;
using System.Text;

namespace solar.models
{
    public class Paged
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string? search { get; set; }
        public string? orderby { get; set; }
    }
}
