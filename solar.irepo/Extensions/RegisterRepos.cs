using solar.generics;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace solar.irepo.Extensions
{
    public static class RegisterRepos
    {
        public static void registerReposDI(ref IServiceCollection services, List<Type> TypesToRegister)
        {
            services.AddScopedDynamic<IAppNotificationRepo>(TypesToRegister);
            services.AddScopedDynamic<IProductRepo>(TypesToRegister);
            services.AddScopedDynamic<ICategoryRepo>(TypesToRegister);
            services.AddScopedDynamic<IUserRepo>(TypesToRegister);
            services.AddScopedDynamic<IAppPermissionRepo>(TypesToRegister);
            services.AddScopedDynamic<IGeneralRepo>(TypesToRegister);
            services.AddScopedDynamic<IProductCategoryRepo>(TypesToRegister);
        }
    }
}
