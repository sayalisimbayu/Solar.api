using solar.generics.Providers;
using solar.github;
using solar.irepo;
using solar.iservices;
using solar.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;

namespace solar.services
{
    public class CategoryService : ICategoryService
    {
        readonly IServicesProvider<ICategoryRepo> _categoryRepo;
        readonly IServicesProvider<IProductCategoryRepo> _productCategoryRepo;
        readonly IServicesProvider<IAppNotificationRepo> _notificationRepo;
        readonly IHttpContextAccessor _accessor;
        readonly IHubContext<NotificationHub> _hubContext;
        public CategoryService(IServicesProvider<ICategoryRepo> categoryRepo,
            IServicesProvider<IProductCategoryRepo> productCategoryRepo,
            IServicesProvider<IAppNotificationRepo> notificationRepo, 
            IHttpContextAccessor accessor,
            IHubContext<NotificationHub> hubContext)
        {
            _accessor = accessor;
            _categoryRepo = categoryRepo;
            _productCategoryRepo = productCategoryRepo;
            _notificationRepo = notificationRepo;
            _hubContext = hubContext;
        }
        public Feedback delete(string tenant, int id, int notificationid)
        {
            Feedback feedback = new Feedback();
            try
            {
                AppNotification notification;
                if (notificationid == 0)
                {
                    notification = new AppNotification()
                    {
                        type = "Category",
                        message = String.Format("Deleting category with id {0}", id),
                        progress = 0
                    };
                    _notificationRepo.GetInstance(tenant).save(ref notification);
                    _hubContext.Clients.All.SendAsync("SendNotification", notification);
                }
                else
                {
                    notification = _notificationRepo.GetInstance(tenant).getById(notificationid);
                }

                try
                {
                    if (_categoryRepo.GetInstance(tenant).delete(id))
                    {
                        notification.progress = 1;
                        notification.message = "Data Deleted Successfully";
                        _notificationRepo.GetInstance(tenant).save(ref notification);
                        feedback = new Feedback
                        {
                            Code = 1,
                            Message = notification.message,
                            data = id
                        };
                        _hubContext.Clients.All.SendAsync("SendNotification", notification);
                    }
                }
                catch (Exception ex)
                {
                    notification.progress = -1;
                    notification.message = "Got the error while removing data";
                    _notificationRepo.GetInstance(tenant).save(ref notification);
                    feedback = new Feedback
                    {
                        Code = 0,
                        Message = "Got the error while removing data",
                        data = ex
                    };
                    GitHub.createIssue(ex, new { id = id, tenant = tenant }, _accessor.HttpContext.Request.Headers);
                }
            }
            catch (Exception ex)
            {
                GitHub.createIssue(ex, new { id = id, tenant = tenant }, _accessor.HttpContext.Request.Headers);
            }
            return feedback;
        }
        public Feedback getById(string tenant, int id)
        {
            Feedback feedback = new Feedback();
            try
            {
                var _categoryData = _categoryRepo.GetInstance(tenant).getById(id);
                if (_categoryData != null)
                {
                    _categoryData.products = _productCategoryRepo.GetInstance(tenant).getByCategoryId(id);
                    feedback = new Feedback
                    {
                        Code = 1,
                        Message = "Data fetched sucessfully",
                        data = _categoryData
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
                    Message = "Got the error while retriving data",
                    data = ex
                };
                GitHub.createIssue(ex, new { id = id, tenant = tenant }, _accessor.HttpContext.Request.Headers);


            }
            return feedback;
        }
        public Feedback getName(string tenant, int id)
        {
            Feedback feedback = new Feedback();
            try
            {
                var _categoryName = _categoryRepo.GetInstance(tenant).getName(id);
                if (!string.IsNullOrEmpty(_categoryName))
                {
                    feedback = new Feedback
                    {
                        Code = 1,
                        Message = "Data fetched sucessfully",
                        data = _categoryName
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
                    Message = "Got the error while retriving data",
                    data = ex
                };
                GitHub.createIssue(ex, new { id = id, tenant = tenant }, _accessor.HttpContext.Request.Headers);

            }
            return feedback;
        }

        public Feedback getPage(string tenant, int start, int number, string searchs, string orderby)
        {
            Feedback feedback = new Feedback();
            try
            {
                var productList = _categoryRepo.GetInstance(tenant).getByPage(start, number, searchs, orderby);
                if (productList != null)
                {
                    feedback = new Feedback
                    {
                        Code = 1,
                        Message = "Data fetched sucessfully",
                        data = productList
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
                    Message = "Got the error while retriving data",
                    data = ex
                };
                GitHub.createIssue(ex, new { tenant = tenant, start = start, number = number, searchs = searchs, orderby = orderby }, _accessor.HttpContext.Request.Headers);
            }
            return feedback;
        }

        public Feedback save(string tenant, Category data)
        {
            Feedback feedback = new Feedback();
            try
            {
                AppNotification notification;
                if (data.notificationid == 0)
                {
                    notification = new AppNotification()
                    {
                        type = "Category",
                        message = "Saving data",
                        progress = 0
                    };
                    _notificationRepo.GetInstance(tenant).save(ref notification);
                    _hubContext.Clients.All.SendAsync("SendNotification", notification);
                }
                else
                {
                    notification = _notificationRepo.GetInstance(tenant).getById(data.notificationid);
                }
                try
                {
                    if (_categoryRepo.GetInstance(tenant).save(data))
                    {
                        notification.progress = 1;
                        notification.message = "Data saved sucessfully for Category: " + data.name;
                        _notificationRepo.GetInstance(tenant).save(ref notification);
                        _hubContext.Clients.All.SendAsync("SendNotification", notification);
                        feedback = new Feedback
                        {
                            Code = 1,
                            Message = "Data saved sucessfully",
                            data = null
                        };
                    }
                }
                catch (Exception ex)
                {
                    notification.progress = -1;
                    notification.message = "Got the error while saving data";
                    _notificationRepo.GetInstance(tenant).save(ref notification);
                    _hubContext.Clients.All.SendAsync("SendNotification", notification);
                    feedback = new Feedback
                    {
                        Code = 0,
                        Message = "Got the error while removing data",
                        data = ex
                    };
                    GitHub.createIssue(ex, new { tenant = tenant, data = data }, _accessor.HttpContext.Request.Headers);

                }
            }
            catch (Exception ex)
            {
                GitHub.createIssue(ex, new { tenant = tenant, data = data }, _accessor.HttpContext.Request.Headers);
            }
            return feedback;
        }

        public Feedback getProducts(string tenant, int id)
        {
            Feedback feedback;
            try
            {
                var _productCategoryData = _productCategoryRepo.GetInstance(tenant).getByCategoryId(id);
                if (_productCategoryData != null)
                {
                    feedback = new Feedback
                    {
                        Code = 1,
                        Message = "Data fetched sucessfully",
                        data = _productCategoryData
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
                    Message = "Got the error while retriving data",
                    data = ex
                };
                GitHub.createIssue(ex, new { id = id, tenant = tenant }, _accessor.HttpContext.Request.Headers);
            }
            return feedback;
        }
    }
}
