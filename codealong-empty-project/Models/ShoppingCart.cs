using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace codealong_pie_project.Models
{
    public class ShoppingCart //samma som för repositories men inget interface
    {
        private readonly AppDbContext appDbContext;

        public string ShoppingCartId { get; set; }

        public List<ShoppingCartItem> ShoppingCartItems { get; set; }

        private ShoppingCart(AppDbContext appDb)
        {
            appDbContext = appDb;
        }

        //ger tillgång till dependency injections / services?
        public static ShoppingCart GetCart(IServiceProvider services)
        {
            //hämta sessionen för den aktiva http-requesten från service providern?
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;

            //hämta manager av typ appdbcontext från service providern
            var context = services.GetService<AppDbContext>();

            //Kollar om värde finns för nyckel CartId (inte null)
            //annars skapar nytt objekt av typen globally unique identifier
            //returns the value of its left-hand operand if it isn't null. else right-hand
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();

            //sätter id:t med nyckel CartId antingen till sig själv eller nytt id
            session.SetString("CartId", cartId);

            //ny implementation av shoppingcart där context= appdbContext
            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }

        public void AddToCart(Pie pie, int amount)
        {
            //shoppingCartItemet där propertyt/objektet Pie har samma PieId som skickat Pie-objekt pie
            //OCH itemets shoppingCartId är samma som ShoppingCartId:t som sattes i GetCart()
            //dvs finns vald item-id i aktuell shoppingCart
            //hämta item-setet från managern, leta upp enda itemet som matchar (eller null)
            var shoppingCartItem =
                    appDbContext.ShoppingCartItems.SingleOrDefault(
                        i => i.Pie.PieId == pie.PieId && i.ShoppingCartId == ShoppingCartId);

            //Om itemet inte finns (i shoppingcarten), skapa ett nytt
            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId, //shoppingcartitems shoppingcart-id = shoppingcarts id
                    Pie = pie,
                    Amount = 1
                };
                //Lägg till i setet/entityt
                appDbContext.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }
            appDbContext.SaveChanges();
        }
        public int RemoveFromCart(Pie pie)
        {
            //samma som add
            var shoppingCartItem =
                    appDbContext.ShoppingCartItems.SingleOrDefault(
                        s => s.Pie.PieId == pie.PieId && s.ShoppingCartId == ShoppingCartId);

            var localAmount = 0;

            //om itemet finns
            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1) //fler än en kvar 2-1
                {
                    shoppingCartItem.Amount--;
                    localAmount = shoppingCartItem.Amount;
                }
                else
                {
                    appDbContext.ShoppingCartItems.Remove(shoppingCartItem); //bara en kvar 1-1
                }
            }

            appDbContext.SaveChanges();

            return localAmount;
        }

        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            //om listan redan finns/skapats returnera lista shoppingcartitems 
            //annars listan = items vars shoppingcart-id= aktuella id och inkludera objektet Pie
            return ShoppingCartItems ??
                   (ShoppingCartItems =
                       appDbContext.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                           .Include(i => i.Pie)
                           .ToList());
        }

        public void ClearCart()
        {
            //hämta aktuell cart med items
            var cartItems = appDbContext
                .ShoppingCartItems
                .Where(cart => cart.ShoppingCartId == ShoppingCartId);

            //ta bort alla cartitems
            appDbContext.ShoppingCartItems.RemoveRange(cartItems);
            //sparas först vid savechanges()
            appDbContext.SaveChanges();
        }

        public decimal GetShoppingCartTotal()
        {
            //hämta totala prissumman (pajpris*st) av alla items i aktuella carten
            var total = appDbContext.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                .Select(i => i.Pie.Price * i.Amount).Sum();
            return total;
        }
    }
}
