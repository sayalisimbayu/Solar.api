using solar.models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;

namespace solar.iservices
{
    public interface ICategoryService
    {
        Feedback getName(string tenant, int id);
        Feedback getById(string tenant, int id);
        Feedback getProducts(string tenant, int id);
        Feedback save(string tenant, Category data);
        Feedback delete(string tenant, int id,int notificationid);
        Feedback getPage(string tenant, int start, int number, string searchs, string orderby);
    }
}
