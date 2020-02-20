using solar.iservices;
using solar.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using solar.generics.Providers;

namespace solar.api.Controllers.api.Controllers
{
    [Route("api/Category")]
    [ApiController]
    public class CategoryController : TBaseController
    {
        private readonly ICategoryService _categoryProvider;
        public CategoryController(IServicesProvider<ICategoryService> categoryProvider)
        {
            _categoryProvider = categoryProvider.GetInstance();
        }

        [HttpGet("{id}")]
        public Feedback Get(int id, string tenant = "")
        {
            return _categoryProvider.getById(tenant, id);
        }

        [HttpGet("{id}/Name"), Authorize]
        public Feedback GetName(int id, string tenant = "")
        {
            return _categoryProvider.getName(tenant, id);
        }

        [HttpGet("{id}/Products"), Authorize]
        public Feedback GetProducts(int id, string tenant = "")
        {
            return _categoryProvider.getProducts(tenant, id);
        }

        [HttpPost, Authorize]
        public Feedback Save(Category data, string tenant = "")
        {
            return _categoryProvider.save(tenant, data);
        }
        [HttpGet("{id}/{notificationid}/Delete"), Authorize]
        public Feedback Delete(int id, int notificationid, string tenant = "")
        {
            return _categoryProvider.delete(tenant, id, notificationid);
        }
        [HttpGet("{start}/{number}/Page"), Authorize]
        public Feedback GetPage(int start, int number, string searchs = "", string orderby = "", string tenant = "")
        {
            return _categoryProvider.getPage(tenant, start, number, searchs, orderby);
        }

    }
}