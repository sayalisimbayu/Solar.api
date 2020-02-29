using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using solar.generics.Providers;
using solar.irepo;
using solar.iservice;
using solar.models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace solar.services
{
    public class ExcelService: IExcelService
    {
        readonly IServicesProvider<ICategoryRepo> _categoryRepo;
        readonly IHttpContextAccessor _accessor;

        public ExcelService(IServicesProvider<ICategoryRepo> categoryRepo,
            IHttpContextAccessor accessor)
        {
            _accessor = accessor;
            _categoryRepo = categoryRepo;
        }

        public Feedback ImportCategoryExcel(string tenant, string fileName)
        {
            Feedback feedback = new Feedback();
            FileInfo file = new FileInfo("C:\\ExcelDemo.xlsx");

            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets["CATEGORY"];
                int totalRows = workSheet.Dimension.Rows;

                List<Category> categoryList = new List<Category>();

                for (int i = 2; i <= totalRows; i++)
                {
                    categoryList.Add(new Category
                    {
                        id = int.Parse(workSheet.Cells[i, 1].Value.ToString()),
                        name = workSheet.Cells[i, 2].Value.ToString(),
                        isdeleted = bool.Parse(workSheet.Cells[i, 3].Value.ToString())
                    });
                }

                //_db.Customers.AddRange(customerList);
                //_db.SaveChanges();
                feedback.data = categoryList;
                return feedback;
            }

        }
    }
}
