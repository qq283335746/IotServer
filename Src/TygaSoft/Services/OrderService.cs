
using System;
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

        public OrderService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<LoginResult> LoginAsync(LoginRequestInfo requestInfo)
        {
            var userInfo = _usersRepository.GetUserInfo(requestInfo.AppId, requestInfo.UserName);
            if (userInfo == null)
            {
                return new LoginResult { ResCode = ResCodeOptions.Error, Message = SR.M_LoginInvalidError };
            }

            var secretPassword = EncryptHelper.EncodePassword(requestInfo.Password, (PasswordFormatOptions)userInfo.PasswordFormat, userInfo.PasswordSalt);
            if (secretPassword != userInfo.Password) return new LoginResult { ResCode = ResCodeOptions.Error, Message = SR.M_LoginInvalidError };

            return new LoginResult { ResCode = ResCodeOptions.Success, Message = SR.Response_Ok };
        }

        // public async Task<int> FindUserAsync(UserRequestInfo requestInfo)
        // {

        // }

        public async Task<int> SaveUserAsync(UserRequestInfo requestInfo)
        {
            var effect = -1;
            var oldUserInfo = _usersRepository.GetUserInfo(requestInfo.ApplicationId, requestInfo.UserName);
            if (oldUserInfo == null)
            {
                var userInfo = new UsersInfo
                {
                    ApplicationId = (int)((ApplicationOptions)requestInfo.ApplicationId),
                    Id = Guid.NewGuid().ToString("N"),
                    Name = requestInfo.UserName,
                    MobilePhone = requestInfo.MobilePhone,
                    Email = requestInfo.Email,
                    PasswordFormat = (int)EncryptHelper.DefaultPasswordFormat,
                    PasswordSalt = EncryptHelper.GenerateSalt(),
                    Roles = JsonConvert.SerializeObject(requestInfo.Roles)
                };
                userInfo.Password = EncryptHelper.EncodePassword(requestInfo.Password, EncryptHelper.DefaultPasswordFormat, userInfo.PasswordSalt);

                effect = await _usersRepository.InsertAsync(userInfo);
            }
            else
            {
                oldUserInfo.MobilePhone = requestInfo.MobilePhone;
                oldUserInfo.Email = requestInfo.Email;
                oldUserInfo.Roles = JsonConvert.SerializeObject(requestInfo.Roles);

                effect = await _usersRepository.UpdateAsync(oldUserInfo);
            }

            return effect;
        }

        public async Task<int> DeleteUserAsync(UserRequestInfo requestInfo)
        {
            var oldUserInfo = _usersRepository.GetUserInfo(requestInfo.ApplicationId,requestInfo.UserName);
            if(oldUserInfo == null) return -1;
            return await _usersRepository.DeleteAsync(oldUserInfo.ApplicationId, oldUserInfo.Name);
        }
    }
}