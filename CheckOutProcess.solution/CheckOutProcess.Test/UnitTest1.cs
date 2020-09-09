using System;
using System.Collections.Generic;
using System.Transactions;
using Xunit;
using CheckOutProcess.Core;

namespace CheckOutProcess.Test
{
    public class UnitTest1
    {
        private Dictionary<Char, int> SKU = new Dictionary<char, int>
        {
            {'A', 50},
            {'B', 30 },
            {'C', 20 },
            {'D', 15 }
        };
        [Fact]
        public void CartTest()
        {
            Cart cart = new Cart();

            Item item1 = new Item() { SKUId = 'A', Quantity = 1, Price = 50 };
            Item item2 = new Item { SKUId = 'B', Quantity = 1, Price = 30 };
            Item item3 = new Item { SKUId = 'C', Quantity = 1, Price = 20 };
            cart.AddItemsToCart(item1);
            cart.AddItemsToCart(item2);
            cart.AddItemsToCart(item3);

            Assert.Equal(3, cart.CountofCartItems());

            var removedItem=cart.RemoveCartItem('B');
            var removedId = removedItem.SKUId;
            Assert.Equal('B', removedId);
            Assert.Equal(2, cart.CountofCartItems());

            var editedItem = cart.EditCart(item3.SKUId,5);
            var editedId = editedItem.SKUId;
            var editedQty = editedItem.Quantity;
            Assert.Equal('C', editedId);
            Assert.Equal(6, editedQty);
            Assert.Equal(2, cart.CountofCartItems());


        }

    }
}
