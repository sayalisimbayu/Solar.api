using solar.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace solar.iservice
{
    public interface IExcelService
    {
        Feedback ImportCategoryExcel(string tenant, string fileName);
    }
}
