using solar.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace solar.irepo
{
    public interface IProductCategoryRepo : IMasterRepo
    {
        string getName(int id);
        ProductCategory getById(int id);
        ProductCategory[] getByProductId(int id);
        ProductCategory[] getByProductIds(int[] ids);
        ProductCategory[] getByCategoryId(int id);
        ProductCategory[] getByCategoryIds(int[] id);
        bool save(ProductCategory data);
        bool delete(int id);
        Tuple<IList<ProductCategory>, int> getByPage(int start, int number, string searchs, string orderby);
    }
}
