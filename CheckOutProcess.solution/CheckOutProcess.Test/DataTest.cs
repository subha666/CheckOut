using CheckOutProcess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace CheckOutProcess.Test
{
    public class DataTest
    {
        [Fact]
        public void AddItemTest()
        {
            InMemoryData data = new InMemoryData();
            data.AddItemsToCart('A', 5);
            var qty = data.GetAllItemsFromCart().FirstOrDefault(i => i.SKUId == 'A').Quantity;
            Assert.Equal(5, qty);
        }
    }
}
