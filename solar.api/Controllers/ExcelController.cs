using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using solar.generics.Providers;
using solar.irepo;
using solar.iservice;
using solar.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace solar.api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelController : ControllerBase
    {
        private readonly IServicesProvider<IExcelService> _importExcelProvider;
        public ExcelController(IServicesProvider<IExcelService> importExcelProvider)
        {
            _importExcelProvider = importExcelProvider;
        }

        [HttpPost]
        public Feedback ImportExcel(string fileName, string tenant = "")
        {
            return _importExcelProvider.GetInstance(tenant).ImportCategoryExcel(tenant, fileName);
        }
    }
}
