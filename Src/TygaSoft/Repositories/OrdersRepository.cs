using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TygaSoft.IRepositories;
using TygaSoft.Model.DbTables;

namespace TygaSoft.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly SqliteContext _context;

        public OrdersRepository(SqliteContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrdersInfo>> FindOrderRouterAsync(string orderCode)
        {
            return await _context.Orders.Where(m => m.OrderCode == orderCode || m.ParentOrderCode == orderCode).ToListAsync();
        }

        public async Task<OrdersInfo> GetOrderInfoAsync(int applicationId, string orderCode)
        {
            return await _context.Orders.SingleOrDefaultAsync(m => m.ApplicationId == applicationId && m.OrderCode == orderCode);
        }

        public async Task<int> DeleteAsync(int applicationId, string orderCode)
        {
            var oldInfo = await _context.Orders.SingleOrDefaultAsync(m => m.ApplicationId == applicationId && m.OrderCode == orderCode);
            if (oldInfo == null) return -1;

            _context.Orders.Remove(oldInfo);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> InsertAsync(OrdersInfo model)
        {
            if (string.IsNullOrEmpty(model.Id)) model.Id = Guid.NewGuid().ToString("N");
            model.CreatedDate = DateTime.Now;
            model.LastUpdatedDate = model.CreatedDate;
            _context.Orders.Add(model);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(OrdersInfo model)
        {
            var oldInfo = await _context.Orders.SingleOrDefaultAsync(m => m.ApplicationId == model.ApplicationId && m.Id == model.Id);
            oldInfo.LastUpdatedDate = DateTime.Now;
            oldInfo.UserId = model.UserId;
            oldInfo.OrderCode = model.OrderCode;
            oldInfo.ParentOrderCode = model.ParentOrderCode;
            oldInfo.TransferItems = model.TransferItems;
            oldInfo.AddItems = model.AddItems;

            // _context.Orders.Remove(model);
            // await _context.Orders.AddAsync(model);
            //_context.Orders.Attach(model);
            //_context.Attach(model);
            //_context.Entry(oldInfo).State = EntityState.Modified;

            // model.LastUpdatedDate = DateTime.Now;
            // _context.Entry(model).Property("Id").IsModified = false;
            // _context.Entry(model).Property("LastUpdatedDate").IsModified = true;
            // _context.Entry(model).Property("OrderCode").IsModified = true;
            // _context.Entry(model).Property("ParentOrderCode").IsModified = true;
            // _context.Entry(model).Property("OrderStatus").IsModified = true;
            // _context.Entry(model).Property("Latlng").IsModified = true;
            // _context.Entry(model).Property("LatlngPlace").IsModified = true;
            // _context.Entry(model).Property("Ip").IsModified = true;
            // _context.Entry(model).Property("IpPlace").IsModified = true;
            // _context.Entry(model).Property("Pictures").IsModified = true;
            // _context.Entry(model).Property("Siblings").IsModified = true;

            _context.Orders.Update(oldInfo);
            return await _context.SaveChangesAsync();
        }
    }
}