using solar.generics.Providers;
using solar.iservices;
using solar.models;
using Microsoft.AspNetCore.Mvc;

namespace solar.api.Controllers.api.Controllers
{
    [Route("api/communi")]
    [ApiController]
    public class NotifyController : ControllerBase
    {
        private readonly IServicesProvider<IAppNotificationService> _notifyProvider;

        public NotifyController(IServicesProvider<IAppNotificationService> notifyProvider)
        {
            _notifyProvider = notifyProvider;
        }
        [HttpPost]
        public Feedback Save(AppNotification data, string tenant = "")
        {
            return _notifyProvider.GetInstance(tenant).save(tenant, data);
        }
        [HttpPost("page")]
        public Feedback Page(Paged page, string tenant = "")
        {
            return _notifyProvider.GetInstance(tenant).getPage(tenant, page);
        }
    }
}