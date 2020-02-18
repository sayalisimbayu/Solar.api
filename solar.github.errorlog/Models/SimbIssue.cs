using System;
using System.Collections.Generic;
using System.Text;

namespace solar.github.errorlog.Models
{
    public class SimbIssue
    {
        public dynamic Headers { get; set; }
        public dynamic input { get; set; }
        public Exception exception { get; set; }
    }
}
