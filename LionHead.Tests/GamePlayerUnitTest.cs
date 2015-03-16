using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LionHead.Core.Entities;
using LionHead.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace LionHead.Tests
{
    [TestClass]
    public class GamePlayerUnitTest
    {

        [TestMethod]
        public void GivenAnIdAndPlayerName_WhenConstructANewItem_ThenItemIsConstructed()
        {
            //Arrange
            var id = 1;
            var playerName = "Test Name";
            
            //Act
            var player = new GamePlayer(id, playerName);

            //Assert
            Assert.IsNotNull(player);
            Assert.AreEqual(id, player.Id);
            Assert.AreEqual(playerName, player.PlayerName);
        }
    }
}
