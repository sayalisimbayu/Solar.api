using solar.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace solar.iservices
{
    public interface IExcelService
    {
        Feedback ImportCategoryExcel(string tenant, string fileName);
    }
}
