using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace codealong_pie_project.Models
{
    public class OrderRepository : IOrderRepository
    {

        private readonly AppDbContext appDbContext;
        private readonly ShoppingCart shoppingCart;

        public OrderRepository(AppDbContext appDb, ShoppingCart cart)
        {
            appDbContext = appDb;
            shoppingCart = cart;
        }

       //skickar in ett order-objekt, sätter värden/props för det
        public void CreateOrder(Order order)
        {
            order.OrderPlaced = DateTime.Now;

            //aktuell shoppingcarts total
            order.OrderTotal = shoppingCart.GetShoppingCartTotal();

            //adding the order with its details
            //orderdetails är en lista av detaljer
            order.OrderDetails = new List<OrderDetail>();


            var shoppingCartItems = shoppingCart.ShoppingCartItems;
            //lägg till orderdetalj för varje cartitem i hämtade cartitems 
            foreach (var shoppingCartItem in shoppingCartItems)
            {
                var orderDetail = new OrderDetail
                {
                    Amount = shoppingCartItem.Amount,
                    PieId = shoppingCartItem.Pie.PieId,
                    Price = shoppingCartItem.Pie.Price
                };

                order.OrderDetails.Add(orderDetail);
            }

            //lägg till ordern i db-ordersetet
            appDbContext.Orders.Add(order);

            appDbContext.SaveChanges();
        }
    }
}
