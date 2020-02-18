using solar.models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace solar.iservices
{
    public interface IProductService
    {
        Feedback getName(string tenant, int id);
        Feedback getById(string tenant, int id);
        Feedback getCategories(string tenant, int id);
        Feedback save(string tenant, Products data);
        Feedback delete(string tenant, int id, int notificationId);
        Feedback getPage(string tenant, int start, int number, string searchs, string orderby);
    }
}
