using Microsoft.AspNetCore.Mvc;
using solar.api.Controllers.api.Controllers;
using solar.generics.Providers;
using solar.iservice;
using solar.models;

namespace solar.api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelController: TBaseController
    {
        private readonly IExcelService _excelProvider;
        public ExcelController(IServicesProvider<IExcelService> excelProvider)
        {
            _excelProvider = excelProvider.GetInstance();
        }

        [HttpPost]
        public Feedback ImportExcel(string fileName, string tenant = "")
        {
            return _excelProvider.ImportCategoryExcel(tenant, fileName);
        }
    }
}
