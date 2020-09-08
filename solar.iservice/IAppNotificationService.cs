using solar.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace solar.iservices
{
    public interface IAppNotificationService
    {
        Feedback save(string tenant, AppNotification data);
        Feedback getById(string tenant, int id);
        Feedback getPage(string tenant, Paged page);
    }
}
