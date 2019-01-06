using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json.Linq;
using TygaSoft.Model;
using TygaSoft.IServices;
using TygaSoft.Services;
using TygaSoft.SysUtility;

namespace TygaSoft.Api.Controllers
{
    [EnableCors("AllowDomain")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IDtoMapper _dtoMapper;

        public OrderController(IOrderService orderService, IDtoMapper dtoMapper)
        {
            _orderService = orderService;
            _dtoMapper = dtoMapper;
        }

        public HelloResult GetHelloAsync()
        {
            return new HelloResult { ResCode = ResCodeOptions.Success, Data = "Hello world", Message = SR.Response_Ok };
        }

        [HttpPost]
        public async Task<LoginResult> LoginAsync([FromBody]LoginRequestInfo requestInfo)
        {
            try
            {
                if (requestInfo == null) return new LoginResult { ResCode = ResCodeOptions.Error, Message = SR.M_InvalidError };

                if (string.IsNullOrEmpty(requestInfo.UserName) || string.IsNullOrEmpty(requestInfo.Password))
                {
                    return new LoginResult { ResCode = ResCodeOptions.Error, Message = SR.M_LoginInvalidError };
                }

                var userInfo = await _orderService.LoginAsync(requestInfo.AppId, requestInfo.AppSecret, requestInfo.DeviceId, requestInfo.UserName, requestInfo.Password);
                if (userInfo == null)
                {
                    return new LoginResult { ResCode = ResCodeOptions.Error, Message = SR.M_LoginInvalidError };
                }

                return new LoginResult { ResCode = userInfo != null ? ResCodeOptions.Success : ResCodeOptions.Error, Message = userInfo != null ? SR.Response_Ok : SR.M_LoginInvalidError, Token = userInfo?.Token, Roles = userInfo?.Roles };
            }
            catch (Exception ex)
            {
                return new LoginResult { ResCode = ResCodeOptions.Error, Message = ex.Message };
            }

        }

        public async Task<FindOrderResult> FindOrderRouterAsync([FromBody]FindOrderRequestInfo requestInfo)
        {
            try
            {
                if (requestInfo == null) return new FindOrderResult { ResCode = ResCodeOptions.Error, Message = SR.M_InvalidError };

                var tokenInfo = SignHelper.UnEncodeToken(requestInfo.Token);
                if (tokenInfo == null || requestInfo.AppId != tokenInfo.AppId)
                {
                    return new FindOrderResult { ResCode = ResCodeOptions.TokenInvalidError, Message = SR.M_LoginRedirect };
                }

                var orders = await _orderService.FindOrderRouterAsync(requestInfo.OrderCode);

                return new FindOrderResult { ResCode = ResCodeOptions.Success, Message = SR.Response_Ok, Orders = orders };
            }
            catch (Exception ex)
            {
                return new FindOrderResult { ResCode = ResCodeOptions.Error, Message = ex.Message };
            }
        }

        [HttpPost]
        public async Task<Result> SaveOrderAsync([FromBody]OrderRequestInfo requestInfo)
        {
            try
            {
                if (requestInfo == null) return new Result { ResCode = ResCodeOptions.Error, Message = SR.M_InvalidError };

                var tokenInfo = SignHelper.UnEncodeToken(requestInfo.Token);
                if (tokenInfo == null || requestInfo.AppId != tokenInfo.AppId)
                {
                    return new Result { ResCode = ResCodeOptions.TokenInvalidError, Message = SR.M_LoginRedirect };
                }

                var userInfo = await _orderService.GetUserInfoAsync(tokenInfo.AppId, tokenInfo.UserId);
                if (userInfo == null)
                {
                    return new Result { ResCode = ResCodeOptions.TokenInvalidError, Message = SR.M_LoginRedirect };
                }

                string orderCode = string.Empty;
                string parentOrderCode = string.Empty;
                string remark = string.Empty;
                string batchRandomCode = string.Empty;
                string latlng = string.Empty;
                string latlngPlace = string.Empty;
                string ip = string.Empty;
                string ipPlace = string.Empty;
                IEnumerable<string> pictures = null;

                if (requestInfo.FunFlag == FunFlagOptions.OrderBack.ToString())
                {
                    orderCode = requestInfo.Data;
                    await _orderService.DoOrderBack(tokenInfo.AppId, tokenInfo.UserId, orderCode);
                    return new Result { ResCode = ResCodeOptions.Success, Message = SR.Response_Ok };
                }

                var jobj = JObject.Parse(requestInfo.Data);
                orderCode = jobj.ContainsKey("OrderCode") ? jobj["OrderCode"].ToString() : string.Empty;
                parentOrderCode = jobj.ContainsKey("ParentOrderCode") ? jobj["ParentOrderCode"].ToString() : string.Empty;
                remark = jobj.ContainsKey("Remark") ? jobj["Remark"].ToString() : string.Empty;
                batchRandomCode = jobj.ContainsKey("BatchRandomCode") ? jobj["BatchRandomCode"].ToString() : string.Empty;

                var userOrderStatus = _orderService.GetOrderStatus(userInfo.Roles);
                if (userOrderStatus == OrderStatusOptions.None)
                {
                    return new Result { ResCode = ResCodeOptions.Error, Message = SR.M_LoginUserNoAccess };
                }

                if (string.IsNullOrEmpty(batchRandomCode) || (string.IsNullOrEmpty(orderCode) && string.IsNullOrEmpty(parentOrderCode)))
                {
                    return new Result { ResCode = ResCodeOptions.Error, Message = SR.M_InvalidError };
                }

                var oldMainOrderInfo = await _orderService.DoMainOrderInfoAsync(tokenInfo.AppId, tokenInfo.UserId, userInfo.UserName, userOrderStatus, orderCode, parentOrderCode, remark, batchRandomCode, pictures, latlng, latlngPlace, ip, ipPlace);
                var isMainOrderChanged = false;

                if (requestInfo.FunFlag == FunFlagOptions.Orders.ToString())
                {
                    //对于每次订单扫描提交，每个用户的BatchRandomCode是唯一的
                    var currentBatchItem = oldMainOrderInfo.TransferItems.FirstOrDefault(m => m.BatchRandomCode == batchRandomCode && m.ByUserId == userInfo.UserId);
                    if (!string.IsNullOrEmpty(parentOrderCode))
                    {
                        if (currentBatchItem != null)
                        {
                            var addItems = currentBatchItem.AddItems.ToList();
                            if (!addItems.Contains(orderCode))
                            {
                                addItems.Add(orderCode);
                                currentBatchItem.AddItems = addItems;

                                isMainOrderChanged = true;
                            }
                        }
                    }
                }
                else if (requestInfo.FunFlag == FunFlagOptions.OrderPackages.ToString())
                {
                    if (!string.IsNullOrEmpty(parentOrderCode))
                    {
                        var oldOrderInfo = await _orderService.GetOrderInfoAsync(tokenInfo.AppId, orderCode);

                        //业务说明：订单打包时，被打包的订单应已经存在
                        if (oldOrderInfo != null)
                        {
                            if (string.IsNullOrEmpty(oldOrderInfo.ParentOrderCode))
                            {
                                oldOrderInfo.ParentOrderCode = oldMainOrderInfo.OrderCode;

                                await _orderService.SaveOrderAsync(oldOrderInfo);
                            }
                        }
                    }
                }

                if (isMainOrderChanged)
                {
                    await _orderService.SaveOrderAsync(oldMainOrderInfo);
                }

                return new Result { ResCode = ResCodeOptions.Success, Message = SR.Response_Ok };
            }
            catch (Exception ex)
            {
                return new Result { ResCode = ResCodeOptions.Error, Message = ex.Message };
            }
        }

        [HttpPost]
        public async Task<UsersResult> GetUserInfoAsync([FromBody]UserRequestInfo requestInfo)
        {
            try
            {
                if (requestInfo == null) return new UsersResult { ResCode = ResCodeOptions.Error, Message = SR.M_InvalidError };

                if (string.IsNullOrEmpty(requestInfo.UserName))
                {
                    return new UsersResult { ResCode = ResCodeOptions.Error, Message = SR.M_InvalidError };
                }

                var oldUserInfo = await _orderService.GetUserInfoAsync(requestInfo.AppId, requestInfo.UserName);

                return new UsersResult { ResCode = ResCodeOptions.Success, Message = SR.Response_Ok, UserInfo = oldUserInfo };
            }
            catch (Exception ex)
            {
                return new UsersResult { ResCode = ResCodeOptions.Error, Message = ex.Message };
            }
        }

        [HttpPost]
        public async Task<Result> SaveUserAsync([FromBody]UserRequestInfo requestInfo)
        {
            try
            {
                if (requestInfo == null) return new UsersResult { ResCode = ResCodeOptions.Error, Message = SR.M_InvalidError };

                if (string.IsNullOrEmpty(requestInfo.UserName) || string.IsNullOrEmpty(requestInfo.Password))
                {
                    return new Result { ResCode = ResCodeOptions.Error, Message = SR.M_InvalidError };
                }

                var effect = await _orderService.SaveUserAsync(_dtoMapper.TMapper<UserInfo>(requestInfo));
                //var effect = await _orderService.SaveUserAsync(requestInfo);

                return new Result { ResCode = effect > 0 ? ResCodeOptions.Success : ResCodeOptions.Error, Message = effect > 0 ? SR.Response_Ok : SR.Response_Error };
            }
            catch (Exception ex)
            {
                return new Result { ResCode = ResCodeOptions.Error, Message = ex.Message };
            }
        }

        [HttpPost]
        public async Task<Result> DeleteUserAsync([FromBody]UserRequestInfo requestInfo)
        {
            try
            {
                if (requestInfo == null) return new UsersResult { ResCode = ResCodeOptions.Error, Message = SR.M_InvalidError };

                if (string.IsNullOrEmpty(requestInfo.UserName))
                {
                    return new Result { ResCode = ResCodeOptions.Error, Message = SR.M_InvalidError };
                }

                var effect = await _orderService.DeleteUserAsync(_dtoMapper.TMapper<UserInfo>(requestInfo));

                return new Result { ResCode = effect > 0 ? ResCodeOptions.Success : ResCodeOptions.Error, Message = effect > 0 ? SR.Response_Ok : SR.Response_Error };
            }
            catch (Exception ex)
            {
                return new Result { ResCode = ResCodeOptions.Error, Message = ex.Message };
            }
        }
    }
}