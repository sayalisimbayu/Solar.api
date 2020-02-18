using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace solar.api.Providers
{
    public interface IServicesProvider<TInterface>
    {
        TInterface GetInstance(string key);
    }
}
