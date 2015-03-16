using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LionHead.Core.Entities;
using LionHead.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace LionHead.Tests
{
    [TestClass]
    public class GameItemUnitTest
    {

        [TestMethod]
        public void GivenAnIdAndItemName_WhenConstructANewItem_ThenItemIsConstructed()
        {
            //Arrange
            var id = 1;
            var itemName = "Test Item";
            
            //Act
            var item = new GameItem(id, itemName);

            //Assert
            Assert.IsNotNull(item);
            Assert.AreEqual(id, item.Id);
            Assert.AreEqual(itemName, item.ItemName);
        }
    }
}
