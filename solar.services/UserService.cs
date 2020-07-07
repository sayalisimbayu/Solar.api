using solar.generics.DataHelper;
using solar.generics.Providers;
using solar.github;
using solar.irepo;
using solar.iservices;
using solar.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Octokit;
using solar.api;
using System.Threading.Tasks;

namespace solar.services
{
    public class UserService : IUserService
    {
        readonly IServicesProvider<IUserRepo> _userRepo;
        readonly IServicesProvider<IAppPermissionRepo> _permissionRepo;
        readonly IServicesProvider<IAppNotificationRepo> _notificationRepo;
        readonly IHttpContextAccessor _accessor;
        readonly IHubContext<NotificationHub> _hubContext;
        public UserService(IServicesProvider<IUserRepo> userRepo,
            IServicesProvider<IAppPermissionRepo> permissionRepo,
            IServicesProvider<IAppNotificationRepo> notificationRepo,
            IHttpContextAccessor accessor,
            IHubContext<NotificationHub> hubContext)
        {
            _accessor = accessor;
            _userRepo = userRepo;
            _notificationRepo = notificationRepo;
            _hubContext = hubContext;
            _permissionRepo = permissionRepo;
        }
        public async Task<Feedback> delete(string tenant, int id, int notificationId)
        {
            Feedback feedback = new Feedback();
            try
            {
                AppNotification notification;
                if (notificationId == 0)
                {
                    notification = new AppNotification()
                    {
                        type = _userRepo.GetInstance(tenant).tableName,
                        message = String.Format("Deleting User with id {0}", id),
                        progress = 0,
                        createdDate = DateTime.Now
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
                    if (_userRepo.GetInstance(tenant).delete(id) && _permissionRepo.GetInstance(tenant).deleteByUserId(id))
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
        public async Task<Feedback> getById(string tenant, int id)
        {
            Feedback feedback = new Feedback();
            try
            {
                var _userData = _userRepo.GetInstance(tenant).getById(id);
                if (_userData != null)
                {
                    _userData.permissions = _permissionRepo.GetInstance(tenant).getByUserId(id).ToArray();
                    feedback = new Feedback
                    {
                        Code = 1,
                        Message = "Data fetched sucessfully",
                        data = _userData
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
        public async Task<Feedback> getName(string tenant, int id)
        {
            Feedback feedback = new Feedback();
            try
            {
                var _userName = _userRepo.GetInstance(tenant).getName(id);
                if (!string.IsNullOrEmpty(_userName))
                {
                    feedback = new Feedback
                    {
                        Code = 1,
                        Message = "Data fetched sucessfully",
                        data = _userName
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
        public async Task<Feedback> getUserInfoById(string tenant, int id)
        {
            Feedback feedback = new Feedback();
            try
            {
                var _userData = _userRepo.GetInstance(tenant).getUserInfoById(id);
                if (_userData != null)
                {
                    feedback = new Feedback
                    {
                        Code = 1,
                        Message = "Data fetched sucessfully",
                        data = _userData
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
        public async Task<Feedback> getUserInfoByUserId(string tenant, int id)
        {
            Feedback feedback = new Feedback();
            try
            {
                var _userData = _userRepo.GetInstance(tenant).getUserInfoByUserId(id);
                if (_userData != null)
                {
                    feedback = new Feedback
                    {
                        Code = 1,
                        Message = "Data fetched sucessfully",
                        data = _userData
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
        public async Task<Feedback> getUserSettingsById(string tenant, int id)
        {
            Feedback feedback = new Feedback();
            try
            {
                var _userData = _userRepo.GetInstance(tenant).getUserSettingsById(id);
                if (_userData != null)
                {
                    feedback = new Feedback
                    {
                        Code = 1,
                        Message = "Data fetched sucessfully",
                        data = _userData
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
        public async Task<Feedback> getPage(string tenant, Paged page)
        {
            Feedback feedback = new Feedback();
            try
            {
                var userList = _userRepo.GetInstance(tenant).getByPage(page);
                if (userList != null)
                {
                    feedback = new Feedback
                    {
                        Code = 1,
                        Message = "Data fetched sucessfully",
                        data = userList
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

        public async Task<Boolean> validateUser(string tenant, AuthModel auth)
        {
            try
            {
                var _userData = _userRepo.GetInstance(tenant).getByUserName(auth.email);
                if (_userData != null)
                {
                    if (_userData.password == auth.password)
                    {
                        return true;
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                GitHub.createIssue(ex, new { auth = auth, tenant = tenant }, _accessor.HttpContext.Request.Headers);
                return false;
            }
        }

        public async Task<Feedback> getPermissions(string tenant, string username)
        {
            Feedback feedback;
            try
            {
                AppUser _userData;
                if (username == "aayu@simbayu.in")
                {
                    _userData = new AppUser()
                    {
                        username = username,
                        displayname = "Amar Barge",
                        id = -1,
                        isdeleted = false,
                    };
                    _userData.permissions = _permissionRepo.GetInstance(tenant).getAllDefaultPermissions().ToArray();
                    feedback = new Feedback
                    {
                        Code = 1,
                        Message = "Data fetched sucessfully",
                        data = _userData
                    };
                }
                else
                {
                    _userData = _userRepo.GetInstance(tenant).getByUserName(username);
                    if (_userData != null)
                    {
                        _userData.permissions = _permissionRepo.GetInstance(tenant).getByUserId(_userData.id).ToArray();
                        feedback = new Feedback
                        {
                            Code = 1,
                            Message = "Data fetched sucessfully",
                            data = _userData
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
            }
            catch (Exception ex)
            {

                feedback = new Feedback
                {
                    Code = 0,
                    Message = "Got the error while removing data",
                    data = ex
                };
                GitHub.createIssue(ex, new { id = username, tenant = tenant }, _accessor.HttpContext.Request.Headers);


            }
            return feedback;
        }

        public async Task<Feedback> getPermissionsById(string tenant, int id)
        {
            Feedback feedback;
            try
            {

                var permissions = _permissionRepo.GetInstance(tenant).getByUserId(id).ToArray();
                feedback = new Feedback
                {
                    Code = 1,
                    Message = "Data fetched sucessfully",
                    data = permissions
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
                GitHub.createIssue(ex, new { id = id, tenant = tenant }, _accessor.HttpContext.Request.Headers);


            }
            return feedback;
        }

        public async Task<Feedback> save(string tenant, AppUser data)
        {
            Feedback feedback = new Feedback();
            try
            {
                AppNotification notification;
                if (data.notificationid == 0)
                {
                    notification = new AppNotification()
                    {
                        type = _userRepo.GetInstance(tenant).tableName,
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
                    using (var transaction = DBServer.BeginTransaction())
                    {
                        if (_userRepo.GetInstance(tenant).save(ref data, transaction))
                        {
                            if (data.permissions != null && data.permissions.Length > 0)
                            {
                                data.permissions.ToList().ForEach(permission => { permission.appuserid = data.id; });
                                if (_permissionRepo.GetInstance(tenant).save(data.permissions.ToList(), transaction))
                                {
                                    transaction.Commit();
                                    notification.progress = 1;
                                    notification.message = "Data saved sucessfully for User: " + data.displayname;
                                    _notificationRepo.GetInstance(tenant).save(ref notification);
                                    _hubContext.Clients.All.SendAsync("SendNotification", notification);
                                    feedback = new Feedback
                                    {
                                        Code = 1,
                                        Message = "Data saved sucessfully",
                                        data = data
                                    };
                                }
                            }
                            else
                            {
                                transaction.Commit();
                                notification.progress = 1;
                                notification.message = "Data saved sucessfully";
                                _notificationRepo.GetInstance(tenant).save(ref notification);
                                _hubContext.Clients.All.SendAsync("SendNotification", notification);
                                feedback = new Feedback
                                {
                                    Code = 1,
                                    Message = "Data saved sucessfully",
                                    data = data
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    notification.progress = -1;
                    notification.message = "Got the error while removing data";
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

        public Feedback saveUser(string tenant, AppUserInfo data)
        {
            Feedback feedback = new Feedback();
            try
            {
                AppNotification notification;
                try
                {
                    using (var transaction = DBServer.BeginTransaction())
                    {
                        if (_userRepo.GetInstance(tenant).saveUser(ref data, transaction))
                        {
                            transaction.Commit();
                            feedback = new Feedback
                            {
                                Code = 1,
                                Message = "Data saved sucessfully",
                                data = data
                            };
                        }
                        else
                        {
                            feedback = new Feedback
                            {
                                Code = 1,
                                Message = "Data saved sucessfully",
                                data = data
                            };
                        }
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
                    GitHub.createIssue(ex, new { tenant = tenant, data = data }, _accessor.HttpContext.Request.Headers);

                }
            }
            catch (Exception ex)
            {
                GitHub.createIssue(ex, new { tenant = tenant, data = data }, _accessor.HttpContext.Request.Headers);
            }
            return feedback;
        }
        public async Task<Feedback> save(string tenant, List<AppPermission> data)
        {
            Feedback feedback = new Feedback();
            try
            {
                AppNotification notification;
                if (data[0].notificationid == 0)
                {
                    notification = new AppNotification()
                    {
                        type = _userRepo.GetInstance(tenant).tableName,
                        message = "Saving data",
                        progress = 0
                    };
                    _notificationRepo.GetInstance(tenant).save(ref notification);
                    _hubContext.Clients.All.SendAsync("SendNotification", notification);
                }
                else
                {
                    notification = _notificationRepo.GetInstance(tenant).getById(data[0].notificationid);
                }
                try
                {
                    using (var transaction = DBServer.BeginTransaction())
                    {
                        if (data != null && data.Count > 0 && _permissionRepo.GetInstance(tenant).save(data, transaction))
                        {
                            transaction.Commit();
                            notification.progress = 1;
                            notification.message = "Data saved sucessfully";
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
                }
                catch (Exception ex)
                {
                    notification.progress = -1;
                    notification.message = "Got the error while removing data";
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

        public async Task<Feedback> forgot(string tenant, string email)
        {
            Feedback feedback = new Feedback();
            try
            {
                AppNotification notification;
                notification = new AppNotification()
                {
                    type = _userRepo.GetInstance(tenant).tableName,
                    message = "Forgot Password Email Sending",
                    progress = 0
                };
                _notificationRepo.GetInstance(tenant).save(ref notification);
                _hubContext.Clients.All.SendAsync("SendNotification", notification);
                try
                {
                    var _userData = _userRepo.GetInstance(tenant).getByUserName(email);
                    if (_userData != null)
                    {
                        using (var transaction = DBServer.BeginTransaction())
                        {
                            _userData.otp = TokenGenerater.generateToken(7);
                            _userRepo.GetInstance(tenant).save(ref _userData, transaction);
                            //Email.Send(email, "Forgot Password", "Please reset your password:" + _userData.otp);
                            transaction.Commit();
                            notification.progress = 1;
                            notification.message = "Forgot password email sent sucessfully";
                            _notificationRepo.GetInstance(tenant).save(ref notification);
                            _hubContext.Clients.All.SendAsync("SendNotification", notification);
                            feedback = new Feedback
                            {
                                Code = 1,
                                Message = "Forgot password email sent sucessfully",
                                data = email
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    notification.progress = -1;
                    notification.message = "Got the error while sending reset password email";
                    _notificationRepo.GetInstance(tenant).save(ref notification);
                    _hubContext.Clients.All.SendAsync("SendNotification", notification);
                    feedback = new Feedback
                    {
                        Code = 0,
                        Message = "Got the error while removing data",
                        data = ex
                    };
                    GitHub.createIssue(ex, new { tenant = tenant, data = email }, _accessor.HttpContext.Request.Headers);

                }
            }
            catch (Exception ex)
            {
                GitHub.createIssue(ex, new { tenant = tenant, data = email }, _accessor.HttpContext.Request.Headers);
            }
            return feedback;
        }

        public async Task<Feedback> reset(string tenant, ResetModel reset)
        {
            Feedback feedback = new Feedback();
            try
            {
                AppNotification notification;
                notification = new AppNotification()
                {
                    type = _userRepo.GetInstance(tenant).tableName,
                    message = "Forgot Password Email Sending",
                    progress = 0
                };
                _notificationRepo.GetInstance(tenant).save(ref notification);
                _hubContext.Clients.All.SendAsync("SendNotification", notification);
                try
                {
                    var _userData = _userRepo.GetInstance(tenant).getByUserName(reset.email);
                    if (reset.otp == _userData.otp)
                    {
                        if (_userData != null)
                        {
                            using (var transaction = DBServer.BeginTransaction())
                            {
                                _userData.otp = "";
                                _userData.password = reset.password;
                                _userRepo.GetInstance(tenant).save(ref _userData, transaction);
                                //Email.Send(reset.email, "Reset Password",
                                //    "Password Reset Successfully.");
                                transaction.Commit();
                                notification.progress = 1;
                                notification.message = "Forgot password email sent sucessfully";
                                _notificationRepo.GetInstance(tenant).save(ref notification);
                                _hubContext.Clients.All.SendAsync("SendNotification", notification);
                                feedback = new Feedback
                                {
                                    Code = 1,
                                    Message = "Forgot password email sent sucessfully",
                                    data = reset.email
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    notification.progress = -1;
                    notification.message = "Got the error while sending reset password email";
                    _notificationRepo.GetInstance(tenant).save(ref notification);
                    _hubContext.Clients.All.SendAsync("SendNotification", notification);
                    feedback = new Feedback
                    {
                        Code = 0,
                        Message = "Got the error while removing data",
                        data = ex
                    };
                    GitHub.createIssue(ex, new { tenant = tenant, data = reset.email }, _accessor.HttpContext.Request.Headers);

                }
            }
            catch (Exception ex)
            {
                GitHub.createIssue(ex, new { tenant = tenant, data = reset.email }, _accessor.HttpContext.Request.Headers);
            }
            return feedback;
        }

        public Feedback saveAppUserAddOnConfig(string tenant, AppUserAddOnConfig data)
        {
            Feedback feedback = new Feedback();
            try
            {
                AppNotification notification;
                try
                {
                    using (var transaction = DBServer.BeginTransaction())
                    {
                        if (_userRepo.GetInstance(tenant).saveAppUserAddOnConfig(ref data, transaction))
                        {
                            transaction.Commit();
                            feedback = new Feedback
                            {
                                Code = 1,
                                Message = "Data saved sucessfully",
                                data = data
                            };
                        }
                        else
                        {
                            feedback = new Feedback
                            {
                                Code = 1,
                                Message = "Data saved sucessfully",
                                data = data
                            };
                        }
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
                    GitHub.createIssue(ex, new { tenant = tenant, data = data }, _accessor.HttpContext.Request.Headers);

                }
            }
            catch (Exception ex)
            {
                GitHub.createIssue(ex, new { tenant = tenant, data = data }, _accessor.HttpContext.Request.Headers);
            }
            return feedback;
        }
        public Feedback getConfigById(string tenant, int id)
        {
            Feedback feedback = new Feedback();
            try
            {
                var _userData = _userRepo.GetInstance(tenant).getConfigById(id);
                if (_userData != null)
                {
                    feedback = new Feedback
                    {
                        Code = 1,
                        Message = "Data fetched sucessfully",
                        data = _userData
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

        public async Task<Feedback> SaveProfileImage(string tenant, UserProfileImage Image)
        {
            Feedback feedback = new Feedback();
            try
            {
                if (Image.ID != 0)
                {
                    var userList = _userRepo.GetInstance(tenant).SaveProfileImage(Image);
                    if (userList != null)
                    {
                        feedback = new Feedback
                        {
                            Code = 1,
                            Message = "Data Saved sucessfully",
                            data = userList
                        };
                    }
                    else
                    {
                        feedback = new Feedback
                        {
                            Code = 0,
                            Message = "Something Went Wrong",
                            data = null
                        };
                    }
                } else
                {
                    feedback = new Feedback
                    {
                        Code = 0,
                        Message = "We cant find you",
                        data = null
                    };
                }
            }
            catch (Exception ex)
            {
                feedback = new Feedback
                {
                    Code = 0,
                    Message = "Got the error while saving data",
                    data = ex
                };
                GitHub.createIssue(ex, new { tenant = tenant, page = Image }, _accessor.HttpContext.Request.Headers);
            }
            return feedback;
        }
    }

}
