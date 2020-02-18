using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace solar.models
{
    public class Feedback
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public dynamic data { get; set; }
    }

    public class Databack
    {
        public DataTable aaData { get; set; }
    }

    public class ListBack
    {
        public DataTable Table { get; set; }
        public int TotalPages { get; set; }
    }
}
