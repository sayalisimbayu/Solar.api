using System;
using System.Collections.Generic;
using System.Text;
using solar.generics.Providers;
using solar.github;
using solar.irepo;
using solar.iservices;
using solar.models;
using Microsoft.AspNetCore.Http;

namespace solar.services
{
    public class GeneralService : IGeneralService
    {
        private readonly IServicesProvider<IGeneralRepo> _generalRepo;
        readonly IHttpContextAccessor _accessor;
        public GeneralService(IServicesProvider<IGeneralRepo> generalRepo, 
            IHttpContextAccessor accessor)
        {
            _accessor = accessor;
            _generalRepo = generalRepo;
        }

        public Feedback getSettings(string tenant)
        {
            Feedback feedback;
            try
            {
                var _userData = _generalRepo.GetInstance(tenant).getAppSettings();

                feedback = new Feedback
                {
                    Code = 0,
                    Message = "Record Fetched Successfully",
                    data = _userData
                };

            }
            catch (Exception ex)
            {

                feedback = new Feedback
                {
                    Code = 0,
                    Message = "Got the error while removing data",
                    data = ex
                };
                GitHub.createIssue(ex, new { tenant = tenant }, _accessor.HttpContext.Request.Headers);


            }
            return feedback;
        }
    }
}
