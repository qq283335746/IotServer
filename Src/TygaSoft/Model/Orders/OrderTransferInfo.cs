using System;
using System.Collections.Generic;

namespace TygaSoft.Model
{
    public class OrderTransferInfo
    {
        public string ByUserId{get;set;}
        public string ByUserName{get;set;}
        public string Remark{get;set;}
        public OrderStatusOptions OrderStatus{get;set;}
        public IEnumerable<string> Pictures{get;set;}
        public string Latlng { get; set; }
        public string LatlngPlace { get; set; }
        public string Ip { get; set; }
        public string IpPlace { get; set; }
        public DateTime LastUpdatedTime{get;set;}
    }
}