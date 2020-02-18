using solar.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace solar.irepo
{
    public interface IAppNotificationRepo : IMasterRepo
    {
        string getName(int id);
        AppNotification getById(int id);
        bool save(ref AppNotification data);
        bool delete(int id);
        Tuple<IList<AppNotification>, int> getByPage(int start, int number, string searchs, string orderby);
    }
}
