using solar.generics.DataHelper;
using solar.generics.Providers;
using solar.github;
using solar.irepo;
using solar.iservices;
using solar.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace solar.services
{
    public class ProductService : IProductService
    {
        readonly IServicesProvider<IProductRepo> _productRepo;
        readonly IServicesProvider<IProductCategoryRepo> _productCategoryRepo;
        readonly IServicesProvider<IAppNotificationRepo> _notificationRepo;
        readonly IHttpContextAccessor _accessor;
        readonly IHubContext<NotificationHub> _hubContext;
        public ProductService(IServicesProvider<IProductRepo> productRepo,
            IServicesProvider<IProductCategoryRepo> productCategoryRepo,
            IServicesProvider<IAppNotificationRepo> notificationRepo, 
            IHttpContextAccessor accessor,
            IHubContext<NotificationHub> hubContext)
        {
            _accessor = accessor;
            _productRepo = productRepo;
            _productCategoryRepo = productCategoryRepo;
            _notificationRepo = notificationRepo;
            _hubContext = hubContext;
        }
        public Feedback delete(string tenant, int id, int notificationId)
        {
            Feedback feedback = new Feedback();
            try
            {
                AppNotification notification;
                if (notificationId == 0)
                {
                    notification = new AppNotification()
                    {
                        type = _productRepo.GetInstance(tenant).tableName,
                        message = String.Format("Deleting Product with id {0}", id),
                        progress = 0
                    };
                    _notificationRepo.GetInstance(tenant).save(ref notification);
                    _hubContext.Clients.All.SendAsync("SendNotification", notification);
                }
                else
                {
                    notification = _notificationRepo.GetInstance(tenant).getById(notificationId);
                }
                try
                {
                        if (_productRepo.GetInstance(tenant).delete(id))
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
            Feedback feedback;
            try
            {
                var _productData = _productRepo.GetInstance(tenant).getById(id);
                if (_productData != null)
                {
                    var _productCategoryData = _productCategoryRepo.GetInstance(tenant).getByProductId(id);
                    _productData.categories = _productCategoryData;
                    feedback = new Feedback
                    {
                        Code = 1,
                        Message = "Data fetched sucessfully",
                        data = _productData
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
        public Feedback getCategories(string tenant, int id)
        {
            Feedback feedback;
            try
            {
                var _productCategoryData = _productCategoryRepo.GetInstance(tenant).getByProductId(id);
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
        public Feedback getName(string tenant, int id)
        {
            Feedback feedback = new Feedback();
            try
            {
                var _productName = _productRepo.GetInstance(tenant).getName(id);
                if (!string.IsNullOrEmpty(_productName))
                {
                    feedback = new Feedback
                    {
                        Code = 1,
                        Message = "Data fetched sucessfully",
                        data = _productName
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
                var productList = _productRepo.GetInstance(tenant).getByPage(start, number, searchs, orderby);
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
                    Message = "Got the error while removing data",
                    data = ex
                };
                GitHub.createIssue(ex, new { tenant = tenant, start = start, number = number, searchs = searchs, orderby = orderby }, _accessor.HttpContext.Request.Headers);
            }
            return feedback;
        }
        public Feedback save(string tenant, Products data)
        {
            Feedback feedback = new Feedback();
            try
            {
                AppNotification notification = new AppNotification()
                {
                    type = "Product",
                    message = "Saving data",
                    progress = 0
                };
                if (data.notificationid == 0)
                {
                    notification = new AppNotification()
                    {
                        type = "Product",
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
                    if (_productRepo.GetInstance(tenant).save(data))
                    {
                        notification.progress = 1;
                        notification.message = "Data saved sucessfully for Product: " + data.name;
                        _notificationRepo.GetInstance(tenant).save(ref notification);
                        feedback = new Feedback
                        {
                            Code = 1,
                            Message = notification.message,
                            data = null
                        };
                        _hubContext.Clients.All.SendAsync("SendNotification", notification);
                    }
                }
                catch (Exception ex)
                {
                    notification.progress = -1;
                    notification.message = "Got the error while saving data";
                    _notificationRepo.GetInstance(tenant).save(ref notification);
                    feedback = new Feedback
                    {
                        Code = 0,
                        Message = notification.message,
                        data = ex
                    };
                    GitHub.createIssue(ex, new { tenant = tenant, data = data }, _accessor.HttpContext.Request.Headers);
                    _hubContext.Clients.All.SendAsync("SendNotification", notification);
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
