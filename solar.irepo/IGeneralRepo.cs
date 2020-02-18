using solar.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace solar.irepo
{
    public interface IGeneralRepo : IMasterRepo
    {
        List<AppSetting> getAppSettings();
    }
}
