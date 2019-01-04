
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TygaSoft.IRepositories;
using TygaSoft.IServices;
using TygaSoft.Model;
using TygaSoft.Model.DbTables;
using TygaSoft.SysUtility;

namespace TygaSoft.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IOrdersRepository _ordersRepository;
        private readonly IDtoMapper _dtoMapper;

        public OrderService(IDtoMapper dtoMapper, IUsersRepository usersRepository, IOrdersRepository ordersRepository)
        {
            _usersRepository = usersRepository;
            _ordersRepository = ordersRepository;
            _dtoMapper = dtoMapper;
        }

        public async Task<OrderInfo> DoMainOrderInfoAsync(int appId, string userId, string userName, OrderStatusOptions userOrderStatus, string orderCode, string parentOrderCode,string remark, IEnumerable<string> pictures, string latlng, string latlngPlace, string ip, string ipPlace)
        {
            OrderInfo mainOrderInfo = null;
            var isChanged = false;

            var transferInfo = new OrderTransferInfo
            {
                ByUserId = userId,
                ByUserName = userName,
                Remark=remark,
                OrderStatus = userOrderStatus,
                Pictures = pictures == null ? new List<string>() : pictures,
                Latlng = latlng,
                LatlngPlace = latlngPlace,
                Ip = ip,
                IpPlace = ipPlace,
                LastUpdatedTime=DateTime.Now
            };

            var transferItems = new List<OrderTransferInfo>();

            //程序应优先保存主订单号
            if (string.IsNullOrEmpty(parentOrderCode))
            {
                mainOrderInfo = await GetOrderInfoAsync(appId, orderCode);
                if (mainOrderInfo == null)
                {
                    transferItems.Add(transferInfo);
                    mainOrderInfo = new OrderInfo
                    {
                        AppId = appId,
                        UserId = userId,
                        OrderCode = orderCode,
                        TransferItems = transferItems,
                        AddItems = new List<OrderAddItemInfo>()
                    };

                    isChanged = true;
                }
                else
                {
                    if (!mainOrderInfo.TransferItems.Any(m => m.OrderStatus == userOrderStatus && m.ByUserId == userId))
                    {
                        transferItems.Add(transferInfo);
                        isChanged = true;
                    }
                }
            }
            else
            {
                mainOrderInfo = await GetOrderInfoAsync(appId, parentOrderCode);
                if (mainOrderInfo == null)
                {
                    throw new ArgumentException(SR.M_InvalidError);
                }
                if (!mainOrderInfo.TransferItems.Any(m => m.OrderStatus == userOrderStatus && m.ByUserId == userId))
                {
                    transferItems.Add(transferInfo);
                    mainOrderInfo.TransferItems = transferItems;
                    isChanged = true;
                }
            }

            if (isChanged) await SaveOrderAsync(mainOrderInfo);

            return mainOrderInfo;
        }

        public OrderStatusOptions GetOrderStatus(IEnumerable<string> Roles)
        {
            if (Roles.Contains(OrderRoleOptions.OrderSended.ToString()))
            {
                return OrderStatusOptions.Send;
            }
            if (Roles.Contains(OrderRoleOptions.OrderPackaged.ToString()))
            {
                return OrderStatusOptions.Package;
            }
            if (Roles.Contains(OrderRoleOptions.OrderEnded.ToString()))
            {
                return OrderStatusOptions.End;
            }

            return OrderStatusOptions.None;
        }

        public async Task<IEnumerable<OrderInfo>> FindOrderRouterAsync(string orderCode)
        {
            var datas = new List<OrderInfo>();

            var orders = await _ordersRepository.FindOrderRouterAsync(orderCode);
            if (orders == null) return datas;

            foreach (var item in orders)
            {
                datas.Add(DtoOrderInfo(item));
            }

            return datas;
        }

        public async Task<OrderInfo> GetOrderInfoAsync(int applicationId, string orderCode)
        {
            var ordersInfo = await _ordersRepository.GetOrderInfoAsync(applicationId, orderCode);
            if (ordersInfo == null) return null;

            return DtoOrderInfo(ordersInfo);
        }

        public async Task<int> SaveOrderAsync(OrderInfo model)
        {
            var effect = -1;
            var ordersInfo = OtdOrderInfo(model);
            
            if (Guid.TryParse(model.OrderId, out var gId) && !gId.Equals(Guid.Empty))
            {
                effect = await _ordersRepository.UpdateAsync(ordersInfo);
            }
            else
            {
                effect = await _ordersRepository.InsertAsync(ordersInfo);
            }

            return effect;
        }

        public async Task<UserInfo> LoginAsync(int appId, string appSecret, string deviceId, string account, string password)
        {
            var oldUserInfo = await _usersRepository.GetUserInfoAsync(appId, account);
            if (oldUserInfo == null)
            {
                return null;
            }

            var secretPassword = EncryptHelper.EncodePassword(oldUserInfo.Password, (PasswordFormatOptions)oldUserInfo.PasswordFormat, oldUserInfo.PasswordSalt);
            if (secretPassword != password) return null;

            var newUserInfo = _dtoMapper.TMapper<UserInfo>(oldUserInfo);
            newUserInfo.Token = SignHelper.EncodeToken(new TokenInfo { AppId = appId, AppSecret = appSecret, DeviceId = deviceId, UserId = newUserInfo.UserId });
            newUserInfo.Roles = oldUserInfo.Roles.ToModel<IEnumerable<string>>();

            return newUserInfo;
        }

        public async Task<UserInfo> GetUserInfoAsync(int applicationId, string name)
        {
            var oldUsersInfo = await _usersRepository.GetUserInfoAsync(applicationId, name);
            if (oldUsersInfo == null) return null;
            var data = _dtoMapper.TMapper<UserInfo>(oldUsersInfo);
            data.Roles = JsonConvert.DeserializeObject<IEnumerable<string>>(oldUsersInfo.Roles);

            return data;
        }

        public async Task<int> SaveUserAsync(UserInfo userInfo)
        {
            var effect = -1;
            var oldUserInfo = await _usersRepository.GetUserInfoAsync(userInfo.ApplicationId, userInfo.UserName);
            if (oldUserInfo == null)
            {
                var currTime = DateTime.Now;
                var newUserInfo = new UsersInfo
                {
                    ApplicationId = (int)((ApplicationOptions)userInfo.ApplicationId),
                    Id = Guid.NewGuid().ToString("N"),
                    Name = userInfo.UserName,
                    MobilePhone = userInfo.MobilePhone,
                    Email = userInfo.Email,
                    PasswordFormat = (int)EncryptHelper.DefaultPasswordFormat,
                    PasswordSalt = EncryptHelper.GenerateSalt(),
                    Roles = JsonConvert.SerializeObject(userInfo.Roles),
                    CreatedDate = currTime,
                    LastUpdatedDate = currTime
                };
                newUserInfo.Password = EncryptHelper.EncodePassword(userInfo.Password, EncryptHelper.DefaultPasswordFormat, userInfo.PasswordSalt);

                effect = await _usersRepository.InsertAsync(newUserInfo);
            }
            else
            {
                oldUserInfo.MobilePhone = userInfo.MobilePhone;
                oldUserInfo.Email = userInfo.Email;
                oldUserInfo.Roles = JsonConvert.SerializeObject(userInfo.Roles);
                oldUserInfo.LastUpdatedDate = DateTime.Now;

                effect = await _usersRepository.UpdateAsync(oldUserInfo);
            }

            return effect;
        }

        public async Task<int> DeleteUserAsync(UserInfo userInfo)
        {
            var oldUserInfo = await _usersRepository.GetUserInfoAsync(userInfo.ApplicationId, userInfo.UserName);
            if (oldUserInfo == null) return -1;

            return await _usersRepository.DeleteAsync(oldUserInfo.ApplicationId, oldUserInfo.Name);
        }

        private OrderInfo DtoOrderInfo(OrdersInfo d)
        {
            var orderInfo = _dtoMapper.TMapper<OrderInfo>(d);
            orderInfo.TransferItems = JsonConvert.DeserializeObject<IEnumerable<OrderTransferInfo>>(d.TransferItems);
            orderInfo.AddItems = JsonConvert.DeserializeObject<IEnumerable<OrderAddItemInfo>>(d.AddItems);

            return orderInfo;
        }

        private OrdersInfo OtdOrderInfo(OrderInfo o)
        {
            var ordersInfo = _dtoMapper.TMapper<OrdersInfo>(o);
            ordersInfo.TransferItems = JsonConvert.SerializeObject(o.TransferItems);
            ordersInfo.AddItems = JsonConvert.SerializeObject(o.AddItems);

            return ordersInfo;
        }
    }
}