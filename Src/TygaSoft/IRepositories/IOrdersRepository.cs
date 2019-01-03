using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TygaSoft.Model.DbTables;

namespace TygaSoft.IRepositories
{
    public interface IOrdersRepository
    {
        Task<IEnumerable<OrdersInfo>> FindOrderRouterAsync(string orderCode);
        Task<OrdersInfo> GetOrderInfoAsync(int applicationId, string orderCode);
        Task<int> DeleteAsync(int applicationId, string orderCode);
        Task<int> InsertAsync(OrdersInfo model);
        Task<int> UpdateAsync(OrdersInfo model);

    }
}