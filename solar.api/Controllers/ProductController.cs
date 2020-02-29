using solar.generics.Providers;
using solar.iservices;
using solar.models;
using Microsoft.AspNetCore.Mvc;

namespace solar.api.Controllers.api.Controllers
{
    [Route("api/Product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IServicesProvider<IProductService> _productProvider;
        public ProductController(IServicesProvider<IProductService> productProvider)
        {
            _productProvider = productProvider;
        }

        [HttpGet("{id}/Name")]
        public Feedback GetName(int id, string tenant = "")
        {
            return _productProvider.GetInstance(tenant).getName(tenant, id);
        }
        [HttpGet("{id}")]
        public Feedback Get(int id, string tenant = "")
        {
            return _productProvider.GetInstance(tenant).getById(tenant, id);
        }

        [HttpGet("{id}/Categories")]
        public Feedback GetCategories(int id, string tenant = "")
        {
            return _productProvider.GetInstance(tenant).getCategories(tenant, id);
        }

        [HttpPost]
        public Feedback Save(Products data, string tenant = "")
        {
            return _productProvider.GetInstance(tenant).save(tenant, data);
        }
        [HttpGet("{id}/{notificationId}/Delete")]
        public Feedback Delete(int id, int notificationId, string tenant = "")
        {
            return _productProvider.GetInstance(tenant).delete(tenant, id, notificationId);
        }
        [HttpPost("Page")]
        public Feedback GetPage(Paged page, string tenant = "")
        {
            return _productProvider.GetInstance(tenant).getPage(tenant, page);
        }
    }
}