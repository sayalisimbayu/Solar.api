using solar.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace solar.irepo
{
    public interface ICategoryRepo : IMasterRepo
    {
        string getName(int id);
        Category getById(int id);
        bool save(Category data);
        bool delete(int id);
        Tuple<IList<Category>, int> getByPage(Paged page);
    }
}
