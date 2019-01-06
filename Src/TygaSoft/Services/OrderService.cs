
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

        public async Task DoOrderBack(int appId, string userId, string orderCode)
        {
            var userLatestRecords = await _ordersRepository.FindUserLatestRecordsAsync(appId, userId);
            if (userLatestRecords == null || !userLatestRecords.Any()) return;

            foreach (var item in userLatestRecords)
            {
                if (item.OrderCode == orderCode)
                {
                    await _ordersRepository.DeleteAsync(appId, orderCode);
                    return;
                }
                var orderInfo = DtoOrderInfo(item);
                var transferItems = orderInfo.TransferItems.OrderByDescending(m => m.LastUpdatedTime);
                foreach (var transfer in transferItems)
                {
                    var addItems = transfer.AddItems.ToList();
                    if (addItems.Any(m => m == orderCode))
                    {
                        addItems.RemoveAll(m => m == orderCode);
                        transfer.AddItems = addItems;

                        await _ordersRepository.UpdateAsync(OtdOrderInfo(orderInfo));

                        return;
                    }

                }
            }
        }

        public async Task<OrderInfo> DoMainOrderInfoAsync(int appId, string userId, string userName, OrderStatusOptions userOrderStatus, string orderCode, string parentOrderCode, string remark, string batchRandomCode, IEnumerable<string> pictures, string latlng, string latlngPlace, string ip, string ipPlace)
        {
            //OrderInfo mainOrderInfo = null;
            var isChanged = false;

            var mainOrderCode = string.IsNullOrEmpty(parentOrderCode) ? orderCode : parentOrderCode;
            var oldMainOrderInfo = await GetOrderInfoAsync(appId, mainOrderCode);

            var transferItems = new List<OrderTransferInfo>{
                    new OrderTransferInfo
                        {
                            ByUserId = userId,
                            ByUserName = userName,
                            Remark = remark,
                            OrderStatus = userOrderStatus,
                            BatchRandomCode = batchRandomCode,
                            Pictures = pictures == null ? new List<string>() : pictures,
                            AddItems = new List<string>(),
                            Latlng = latlng,
                            LatlngPlace = latlngPlace,
                            Ip = ip,
                            IpPlace = ipPlace,
                            LastUpdatedTime = DateTime.Now
                        }
                };

            if (oldMainOrderInfo == null)
            {
                oldMainOrderInfo = new OrderInfo
                {
                    AppId = appId,
                    UserId = userId,
                    OrderCode = orderCode,
                    Status = userOrderStatus,
                    TransferItems = transferItems
                };

                isChanged = true;
            }
            else
            {
                var currentBatchItem = oldMainOrderInfo.TransferItems.FirstOrDefault(m => m.BatchRandomCode == batchRandomCode && m.ByUserId == userId);
                if (currentBatchItem == null)
                {
                    var oldTransferItems = oldMainOrderInfo.TransferItems.ToList();
                    oldTransferItems.AddRange(transferItems);
                    oldMainOrderInfo.TransferItems = oldTransferItems;
                    isChanged = true;
                }
            }

            if (isChanged) await SaveOrderAsync(oldMainOrderInfo);

            return oldMainOrderInfo;
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

            return orderInfo;
        }

        private OrdersInfo OtdOrderInfo(OrderInfo o)
        {
            var ordersInfo = _dtoMapper.TMapper<OrdersInfo>(o);
            ordersInfo.TransferItems = JsonConvert.SerializeObject(o.TransferItems);

            return ordersInfo;
        }
    }
}