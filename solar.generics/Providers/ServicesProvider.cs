using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace solar.generics.Providers
{
    public class ServicesProvider<TInterface> : IServicesProvider<TInterface>
    {
        private IHttpContextAccessor _httpContextAccessor;

        public ServicesProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            GetInstance();
        }

        public TInterface GetInstance()
        {
            string CorrelationId = 
                _httpContextAccessor.HttpContext?.Request.Headers["x-correlationid"] ?? string.Empty;
            
            var func = this.GetService();
            return func(CorrelationId);
        }

        public TInterface GetInstance(string key)
        {
            var func = this.GetService();
            return func(key);
        }

        private Func<string, TInterface> GetService()
        {
            return (Func<string, TInterface>)_httpContextAccessor.HttpContext.RequestServices.GetService(typeof(Func<string, TInterface>));
        }
    }
}
