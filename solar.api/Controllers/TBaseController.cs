using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace solar.api.Controllers.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TBaseController : ControllerBase
    {
        protected string CorrelationId =>
            HttpContext?.Request.Headers["x-correlationid"] ?? string.Empty;
    }
}