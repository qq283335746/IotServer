using System;
using System.Collections.Generic;

namespace TygaSoft.Model
{
    public class OrderInfo
    {
        public int AppId { get; set; }
        public string UserId{get;set;}
        public string OrderId { get; set; }
        public string OrderCode { get; set; }
        public string ParentOrderCode { get; set; }
        public IEnumerable<OrderTransferInfo> TransferItems{get;set;}
        public IEnumerable<OrderAddItemInfo> AddItems{get;set;}
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        
    }
}