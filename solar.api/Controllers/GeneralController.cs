using solar.generics.Providers;
using solar.iservices;
using solar.models;
using Microsoft.AspNetCore.Mvc;

namespace solar.api.Controllers.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralController : ControllerBase
    {
        private readonly IServicesProvider<IGeneralService> _generalProvider;

        public GeneralController(IServicesProvider<IGeneralService> generalProvider)
        {
            _generalProvider = generalProvider;
        }

        [HttpGet("Settings")]
        public Feedback GetSettings(string tenant = "")
        {
            return _generalProvider.GetInstance(tenant).getSettings(tenant);
        }
    }
}