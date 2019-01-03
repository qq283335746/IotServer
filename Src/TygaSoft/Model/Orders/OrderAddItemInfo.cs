using System;

namespace TygaSoft.Model{
    public class OrderAddItemInfo
    {
        public string ByUserId{get;set;}
        public string ByUserName{get;set;}
        public string OrderCode{get;set;}
        public OrderStatusOptions OrderStatus{get;set;}
        public string Latlng { get; set; }
        public string LatlngPlace { get; set; }
        public string Ip { get; set; }
        public string IpPlace { get; set; }
    }
}