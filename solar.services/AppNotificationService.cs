using solar.generics.Providers;
using solar.github;
using solar.irepo;
using solar.iservices;
using solar.models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace solar.services
{
    public class AppNotificationService : IAppNotificationService
    {
        readonly IServicesProvider<IAppNotificationRepo> _notificationRepo;
        readonly IHttpContextAccessor _accessor;
        public AppNotificationService(IServicesProvider<IAppNotificationRepo> notificationRepo, 
            IHttpContextAccessor accessor)
        {
            _notificationRepo = notificationRepo;
            _accessor = accessor;
        }

        public Feedback getById(string tenant, int id)
        {
            Feedback feedback = new Feedback();
            try
            {
                var _notificationData = _notificationRepo.GetInstance(tenant).getById(id);
                if (_notificationData != null)
                {
                    feedback = new Feedback
                    {
                        Code = 1,
                        Message = "Data fetched sucessfully",
                        data = _notificationData
                    };
                }
                else
                {
                    feedback = new Feedback
                    {
                        Code = 0,
                        Message = "Record not found",
                        data = null
                    };
                }
            }
            catch (Exception ex)
            {

                feedback = new Feedback
                {
                    Code = 0,
                    Message = "Got the error while removing data",
                    data = ex
                };
                GitHub.createIssue(ex, new { id = id, tenant = tenant }, _accessor.HttpContext.Request.Headers);


            }
            return feedback;
        }

        public Feedback getPage(string tenant, Paged page)
        {
            Feedback feedback = new Feedback();
            try
            {
                var _notificationList = _notificationRepo.GetInstance(tenant).getByPage(page);
                if (_notificationList != null)
                {
                    feedback = new Feedback
                    {
                        Code = 1,
                        Message = "Data fetched sucessfully",
                        data = _notificationList
                    };
                }
                else
                {
                    feedback = new Feedback
                    {
                        Code = 0,
                        Message = "Record not found",
                        data = null
                    };
                }
            }
            catch (Exception ex)
            {
                feedback = new Feedback
                {
                    Code = 0,
                    Message = "Got the error while removing data",
                    data = ex
                };
                GitHub.createIssue(ex, new { tenant = tenant, page = page }, _accessor.HttpContext.Request.Headers);
            }
            return feedback;
        }

        public Feedback save(string tenant, AppNotification data)
        {
            Feedback feedback = new Feedback();
            try
            {
                if (_notificationRepo.GetInstance(tenant).save(ref data))
                {
                    feedback = new Feedback
                    {
                        Code = 1,
                        Message = "Data saved sucessfully",
                        data = data
                    };
                }
            }
            catch (Exception ex)
            {
                GitHub.createIssue(ex, new { tenant = tenant, data = data }, _accessor.HttpContext.Request.Headers);
            }
            return feedback;
        }
    }
}
