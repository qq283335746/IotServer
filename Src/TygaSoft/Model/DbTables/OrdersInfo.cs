using System;

namespace TygaSoft.Model.DbTables
{
    public class OrdersInfo
    {
        public string Id { get; set; }
        public int ApplicationId { get; set; }
        public string UserId { get; set; }
        public string OrderCode { get; set; }
        public string ParentOrderCode { get; set; }
        public OrderStatusOptions Status { get; set; }

        //对应IEnumerable<OrderTransferInfo> TransferItems
        public string TransferItems { get; set; }

        //对应IEnumerable<OrderAddItemInfo> AddItems
        // public string AddItems { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
}