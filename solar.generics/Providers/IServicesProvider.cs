using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace solar.generics.Providers
{
    public interface IServicesProvider<TInterface>
    {
        TInterface GetInstance();
        TInterface GetInstance(string key);
    }
}
