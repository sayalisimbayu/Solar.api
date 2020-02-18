using System;
using System.Collections.Generic;
using System.Text;
using solar.models;

namespace solar.iservices
{
    public interface IGeneralService
    {
        Feedback getSettings(string tenant);
    }
}
