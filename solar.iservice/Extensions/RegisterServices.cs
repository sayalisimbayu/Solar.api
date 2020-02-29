using solar.generics;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using solar.iservice;
namespace solar.iservices.Extensions
{
    public static class RegisterServices
    {
        public static void registerServicesDI(ref IServiceCollection services,List<Type> TypesToRegister)
        {
            // Multitenant interface with its related classes
            services.AddScopedDynamic<IProductService>(TypesToRegister);
            services.AddScopedDynamic<ICategoryService>(TypesToRegister);
            services.AddScopedDynamic<IAppNotificationService>(TypesToRegister);
            services.AddScopedDynamic<IUserService>(TypesToRegister);
            services.AddScopedDynamic<IGeneralService>(TypesToRegister);
            services.AddScopedDynamic<IExcelService>(TypesToRegister);
        }
    }
}
