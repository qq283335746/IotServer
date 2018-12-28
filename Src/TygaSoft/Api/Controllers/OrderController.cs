using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using TygaSoft.Model;
using TygaSoft.IServices;
using TygaSoft.Services;

namespace TygaSoft.Api.Controllers
{
    [EnableCors("AllowDomain")]
    public class OrderController : Controller
    {
        //private readonly IRailwayService _railwayService;
        // public RailwayController(IRailwayService railwayService)
        // {
        //     _railwayService = railwayService;
        // }

        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public HelloResult GetHelloAsync()
        {
            return new HelloResult { ResCode = ResCodeOptions.Success, Data = "Hello world", Message = SR.Response_Ok };
        }

        [HttpPost]
        public LoginResult LoginAsync([FromBody]LoginRequestInfo requestInfo)
        {
            if (string.IsNullOrEmpty(requestInfo.UserName) || string.IsNullOrEmpty(requestInfo.Password))
            {
                return new LoginResult { ResCode = ResCodeOptions.Error, Message = SR.M_LoginInvalidError };
            }

            return new LoginResult { ResCode = ResCodeOptions.Success };
        }

        // [HttpPost]
        // public async Task<LoginResult> LoginAsync([FromBody]LoginRequestInfo requestInfo)
        // {
        //     return await _orderService.Login(requestInfo);
        // }

        // [HttpPost]
        // public async Task<Result> FindUserAsync([FromBody]UserRequestInfo requestInfo)
        // {
        //     try
        //     {
        //         if (string.IsNullOrEmpty(requestInfo.UserName))
        //         {
        //             return new Result { ResCode = ResCodeOptions.Error, Message = SR.M_InvalidError };
        //         }

        //         var effect = await _orderService.SaveUserAsync(requestInfo);

        //         return new Result { ResCode = effect > 0 ? ResCodeOptions.Success : ResCodeOptions.Error, Message = effect > 0 ? SR.Response_Ok : SR.Response_Error };
        //     }
        //     catch (Exception ex)
        //     {
        //         return new Result { ResCode = ResCodeOptions.Error, Message = ex.Message };
        //     }
        // }

        [HttpPost]
        public async Task<Result> SaveUserAsync([FromBody]UserRequestInfo requestInfo)
        {
            try
            {
                if (string.IsNullOrEmpty(requestInfo.UserName) || string.IsNullOrEmpty(requestInfo.Password))
                {
                    return new Result { ResCode = ResCodeOptions.Error, Message = SR.M_InvalidError };
                }

                var effect = await _orderService.SaveUserAsync(requestInfo);

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
                if (string.IsNullOrEmpty(requestInfo.UserName))
                {
                    return new Result { ResCode = ResCodeOptions.Error, Message = SR.M_InvalidError };
                }

                var effect = await _orderService.DeleteUserAsync(requestInfo);

                return new Result { ResCode = effect > 0 ? ResCodeOptions.Success : ResCodeOptions.Error, Message = effect > 0 ? SR.Response_Ok : SR.Response_Error };
            }
            catch (Exception ex)
            {
                return new Result { ResCode = ResCodeOptions.Error, Message = ex.Message };
            }
        }
    }
}