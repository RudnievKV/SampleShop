using System;
using System.Collections.Generic;
using System.Linq;
using SampleShop.Interfaces;
using SampleShop.Model;
using SampleShop.Queries;

namespace SampleShop.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly GetAllOrdersQuery queryAll;
        private readonly GetAllItemsQuery queryAllItems;
        private readonly GetOrderByIdQuery queryById;
        private readonly AddOrderQuery queryAdd;
        private readonly DeleteOrderQuery queryDelete;


        public OrdersService(GetAllOrdersQuery queryAllOrders, GetAllItemsQuery queryAllItems,
                             GetOrderByIdQuery queryById, AddOrderQuery queryAdd,
                             DeleteOrderQuery queryDelete)
        {
            this.queryAll = queryAllOrders;
            this.queryAllItems = queryAllItems;
            this.queryById = queryById;
            this.queryAdd = queryAdd;
            this.queryDelete = queryDelete;
        }

        /// <summary>
        /// Lists all orders that exist in db
        /// </summary>
        public IEnumerable<Order> All()
        {
            return queryAll.Execute().ToList();
        }

        /// <summary>
        /// Gets single order by its id
        /// </summary>
        public Order GetById(int id)
        {
            return queryById.Execute(id);
        }

        /// <summary>
        /// Tries to add given order to db, after validating it
        /// </summary>
        public Order Add(Order newOrder)
        {
            if (newOrder == null)
            {
                throw new ArgumentNullException("newOrder");
            }

            var result = ValidateNewOrder(newOrder);
            if ((result & ValidationResult.Ok) == ValidationResult.Ok)
            {
                queryAdd.Execute(newOrder);
                return newOrder;
            }

            return null;
        }

        /// <summary>
        /// Checks whether given order can be added.
        /// Performs logical and business validation.
        /// </summary>
        public ValidationResult ValidateNewOrder(Order newOrder)
        {
            var result = ValidationResult.Default;

            if (newOrder == null)
            {
                throw new ArgumentNullException("newOrder");
            }

            var items = queryAllItems.Execute();

            foreach (var item in newOrder.OrderItems)
            {
                if (item.Value <= 0) result |= ValidationResult.NoItemQuantity;
                if (!items.Any(p => p.Id == item.Key))
                {
                    result |= ValidationResult.ItemDoesNotExist;
                }
            }

            if (result == ValidationResult.Default)
            {
                result = ValidationResult.Ok;
            }

            return result;
        }

        /// <summary>
        /// Deletes (if exists) order from db (by its id)
        /// </summary>
        public void Delete(int id)
        {
            queryDelete.Execute(id);
        }

        /// <summary>
        /// Returns all orders (listed chronologically) between a given start date and end date.
        /// Start and end dates must be from the past (not in the future or today).
        /// </summary>
        public IEnumerable<Order> GetByDates(DateTime start, DateTime end)
        {
            var today = DateTime.Today;

            if (today <= start || today <= end)
            {
                throw new ArgumentException();
            }

            var orders = queryAll.Execute()
                .Where(order => order.CreateDate > start && order.CreateDate < end)
                .OrderBy(order => order.CreateDate)
                .ToList();

            return orders;

        }

        /// <summary>
        /// Returns the list of items sold across all orders in a given day with
        /// the total revenue
        /// Day must be from the past (not in the future or today).
        /// </summary>
        public IEnumerable<ItemSoldStatistics> GetItemsSoldByDay(DateTime day)
        {

            var today = DateTime.Today;

            if (today <= day)
            {
                throw new ArgumentException();
            }

            var orders = queryAll.Execute()
                .Where(s => s.CreateDate.Date == day.Date)
                .ToList();

            Dictionary<int, int> orderItemsCombined = CombineOrderItems(orders);

            var items = queryAllItems.Execute().Where(s => orderItemsCombined.ContainsKey(s.Id)).ToList();

            List<ItemSoldStatistics> itemSoldStatisticsCombined = CombineItemStatistics(orderItemsCombined, items);

            return itemSoldStatisticsCombined;
        }

        private Dictionary<int, int> CombineOrderItems(List<Order> orders)
        {
            var orderItemsCombined = new Dictionary<int, int>();
            foreach (var order in orders)
            {
                foreach (var item in order.OrderItems)
                {
                    if (orderItemsCombined.ContainsKey(item.Key))
                        orderItemsCombined[item.Key] = orderItemsCombined[item.Key] + item.Value;
                    else
                        orderItemsCombined[item.Key] = item.Value;
                }
            }

            return orderItemsCombined;
        }

        private List<ItemSoldStatistics> CombineItemStatistics(Dictionary<int, int> dictionary, List<Item> items)
        {
            var itemSoldStatistics = new List<ItemSoldStatistics>();
            foreach (var item in items)
            {
                var itemStatistics = new ItemSoldStatistics()
                {
                    ItemId = item.Id,
                    Total = dictionary[item.Id] * item.Price
                };
                itemSoldStatistics.Add(itemStatistics);
            }

            return itemSoldStatistics;
        }
    }
}
