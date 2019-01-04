
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TygaSoft.Model;

namespace TygaSoft.IServices
{
    public interface IOrderService
    {
        #region Orders

        Task<OrderInfo> DoMainOrderInfoAsync(int appId, string userId, string userName, OrderStatusOptions userOrderStatus, string orderCode, string parentOrderCode,string remark, IEnumerable<string> pictures, string latlng, string latlngPlace, string ip, string ipPlace);

        OrderStatusOptions GetOrderStatus(IEnumerable<string> Roles);

        Task<IEnumerable<OrderInfo>> FindOrderRouterAsync(string orderCode);

        Task<OrderInfo> GetOrderInfoAsync(int applicationId, string orderCode);

        Task<int> SaveOrderAsync(OrderInfo model);

        #endregion

        #region Users

        Task<UserInfo> LoginAsync(int appId, string appSecret, string deviceId, string account, string password);
        Task<UserInfo> GetUserInfoAsync(int applicationId, string name);
        Task<int> SaveUserAsync(UserInfo userInfo);
        Task<int> DeleteUserAsync(UserInfo userInfo);

        #endregion
    }
}