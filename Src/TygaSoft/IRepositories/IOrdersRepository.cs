using System;
using System.Threading.Tasks;
using TygaSoft.Model.DbTables;

namespace TygaSoft.IRepositories
{
    public interface IOrdersRepository
    {
        Task<OrdersInfo> GetOrderInfoAsync(int applicationId, string orderCode);
        Task<int> DeleteAsync(int applicationId, string orderCode);
        Task<int> InsertAsync(OrdersInfo model);
        Task<int> UpdateAsync(OrdersInfo model);

    }
}