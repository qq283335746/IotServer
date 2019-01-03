using System;
using System.Collections.Generic;
using AutoMapper;

namespace TygaSoft.Model
{
    public class DtoProfile : Profile
    {
        public DtoProfile()
        {
            CreateMap<OrderInfo, DbTables.OrdersInfo>()
            .ForMember(d => d.Id, opt => opt.MapFrom(o => o.OrderId))
            .ForMember(d => d.ApplicationId, opt => opt.MapFrom(o => o.AppId))
            .ForMember(d => d.TransferItems, opt => opt.Ignore())
            .ForMember(d => d.AddItems, opt => opt.Ignore())
            ;

             CreateMap<DbTables.OrdersInfo,OrderInfo>()
            .ForMember(d => d.OrderId, opt => opt.MapFrom(o => o.Id))
            .ForMember(d => d.AppId, opt => opt.MapFrom(o => o.ApplicationId))
            .ForMember(d => d.TransferItems, opt => opt.Ignore())
            .ForMember(d => d.AddItems, opt => opt.Ignore())
            ;

            CreateMap<UserInfo, DbTables.UsersInfo>()
            .ForMember(d => d.Id, opt => opt.MapFrom(o => o.UserId))
            .ForMember(d => d.Name, opt => opt.MapFrom(o => o.UserName))
            .ForMember(d => d.Roles, opt => opt.Ignore())
            ;

            CreateMap<DbTables.UsersInfo, UserInfo>()
           .ForMember(d => d.UserId, opt => opt.MapFrom(o => o.Id))
           .ForMember(d => d.UserName, opt => opt.MapFrom(o => o.Name))
           .ForMember(d => d.Roles, opt => opt.Ignore())
           ;

            CreateMap<LoginRequestInfo, UserInfo>()
            .ForMember(d => d.ApplicationId, opt => opt.MapFrom(o => o.AppId))
            ;
            CreateMap<UserRequestInfo, UserInfo>()
            .ForMember(d => d.ApplicationId, opt => opt.MapFrom(o => o.AppId))
            ;
            CreateMap<UserInfo, UserRequestInfo>()
            .ForMember(d => d.AppId, opt => opt.MapFrom(o => o.ApplicationId))
            ;
        }
    }
}