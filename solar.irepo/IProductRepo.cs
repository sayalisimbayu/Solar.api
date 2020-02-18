using solar.models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace solar.irepo
{
    public interface IProductRepo : IMasterRepo
    {
        string getName(int id);
        Products getById(int id);
        bool save(Products data);
        bool delete(int id);
        Tuple<IList<Products>, int> getByPage(int start, int number, string searchs, string orderby);
    }
}
