using System;
using System.Collections.Generic;
using System.Text;

namespace solar.models
{
    public class AppNotification
    {
        public int id { get; set; }
        public string type { get; set; }
        public string message { get; set; }
        public int progress { get; set; }
        public bool isRead { get; set; }
        public bool isDeleted { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime updatedDate { get; set; }
        public int userId { get; set; }
    }
}
