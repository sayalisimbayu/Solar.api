using Microsoft.AspNetCore.Hosting;
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
        private readonly IHostingEnvironment _hostingEnvironment;
        readonly IServicesProvider<ICategoryRepo> _categoryRepo;
        readonly IHttpContextAccessor _accessor;

        public ExcelService(IServicesProvider<ICategoryRepo> categoryRepo,
            IHttpContextAccessor accessor, IHostingEnvironment hostingEnvironment)
        {
            _accessor = accessor;
            _categoryRepo = categoryRepo;
            _hostingEnvironment = hostingEnvironment;
        }

        public Feedback ImportCategoryExcel(string tenant, ImportExcel uploadedFile)
        {
            string rootFolder = _hostingEnvironment.WebRootPath;
            Feedback feedback = new Feedback();
            FileInfo file = new FileInfo(Path.Combine(rootFolder, "uploadedFiles", @uploadedFile.fileName));

            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets[uploadedFile.type];
                int totalRows = workSheet.Dimension.Rows;

                switch(uploadedFile.type)
                {
                    case "CATEGORY":
                        feedback.data = CategoryFile(totalRows, workSheet);
                        break;
                    case "PRODUCTS":
                        feedback.data = ProductFile(totalRows, workSheet);
                        break;
                }
                return feedback;
            }

        }

        private List<Category> CategoryFile(int totalRows, ExcelWorksheet workSheet)
        {
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

            return categoryList;
        }

        private List<Products> ProductFile(int totalRows, ExcelWorksheet workSheet)
        {
            List<Products> productList = new List<Products>();

            for (int i = 2; i <= totalRows; i++)
            {
                productList.Add(new Products
                {
                    id = int.Parse(workSheet.Cells[i, 1].Value.ToString()),
                    name = workSheet.Cells[i, 2].Value.ToString(),
                    isdeleted = bool.Parse(workSheet.Cells[i, 3].Value.ToString())
                });
            }

            return productList;
        }
    }
}
