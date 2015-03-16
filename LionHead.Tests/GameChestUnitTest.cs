using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LionHead.Core.Entities;
using LionHead.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace LionHead.Tests
{
    [TestClass]
    public class GameChestUnitTest
    {
        [TestMethod]
        public void GivenAChest_WhenIAddItemToLootTableWithDropChanceZero_ThenTheLootTableContainsItem()
        {
            //Arrange
            var chest = new GameChest();
            var item = new GameItem(1, "Test Item");
            var dropChance = 0U;
            //Act
            chest.AddOrUpdateLootTableItem(item, dropChance);

            //Assert
            Assert.IsNotNull(chest.LootTable.FirstOrDefault());
            Assert.AreEqual(item, chest.LootTable.First().Key);
            Assert.AreEqual(dropChance, chest.LootTable.First().Value);
        }

        [TestMethod]
        public void GivenAChest_WhenIAddItemToLootTableWithDropChance100_ThenTheLootTableContainsItem()
        {
            //Arrange
            var chest = new GameChest();
            var item = new GameItem(1, "Test Item");
            var dropChance = 100U;
            //Act
            chest.AddOrUpdateLootTableItem(item, dropChance);

            //Assert
            Assert.IsNotNull(chest.LootTable.FirstOrDefault());
            Assert.AreEqual(item, chest.LootTable.First().Key);
            Assert.AreEqual(dropChance, chest.LootTable.First().Value);
        }

        [TestMethod]
        public void GivenAChest_WhenIAddItemToLootTableWithDropChance101_ThenExceptionIsThrown()
        {
            //Arrange
            var chest = new GameChest();
            var item = new GameItem(1, "Test Item");
            var dropChanceItem = 101U;
            Exception result = null;
            //Act
            try
            {
                chest.AddOrUpdateLootTableItem(item, dropChanceItem);
            }
            catch (Exception ex)
            {
                result = ex;
            }
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(string.Format("Max drop chance allowed to be added is {0}.", 100), result.Message);
            Assert.AreEqual(typeof(InvalidOperationException), result.GetType());
        }

        [TestMethod]
        public void GivenAChestAndItemWithDropChanceOf100_WhenIAddSameItemToLootTableWithDropChance50_ThenTheLootTableIsUpdated()
        {
            //Arrange
            var chest = new GameChest();
            var item = new GameItem(1, "Test Item");
            var dropChance = 50U;
            chest.AddOrUpdateLootTableItem(item, 100U);
            //Act
            chest.AddOrUpdateLootTableItem(item, dropChance);

            //Assert
            Assert.IsNotNull(chest.LootTable.FirstOrDefault());
            Assert.AreEqual(item, chest.LootTable.First().Key);
            Assert.AreEqual(dropChance, chest.LootTable.First().Value);
        }

        [TestMethod]
        public void GivenAChestWithItemDropChanceOf50_WhenIConfigureTheItemDropChanceTo70_ThenExceptionIsThrown()
        {
            //Arrange
            var chest = new GameChest();
            var item1 = new GameItem(1, "Test Item1");
            var item2 = new GameItem(1, "Test Item2");
            var dropChanceItem1 = 50U;
            var dropChanceItem2 = 70U;
            chest.AddOrUpdateLootTableItem(item1, dropChanceItem1);
            Exception result = null;
            //Act
            try
            {
                chest.AddOrUpdateLootTableItem(item2, dropChanceItem2);
            }
            catch (Exception ex)
            {
                result = ex;
            }
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(string.Format("Max drop chance allowed to be added is {0}.", 100 - dropChanceItem1), result.Message);
            Assert.AreEqual(typeof(InvalidOperationException), result.GetType());
        }

        [TestMethod]
        public void GivenAChestWithNoItems_WhenIRemoveAItemFromLootTable_ThenNothingHappens()
        {
            //Arrange
            var chest = new GameChest();
            //Act
            chest.DeleteIfExistsLootTableItem(new GameItem(1, "Some Item"));

            //Assert
            Assert.AreEqual(0, chest.LootTable.Count());
        }

        [TestMethod]
        public void GivenAChestWithItem1_WhenIRemoveItem2FromLootTable_ThenNothingHappens()
        {
            //Arrange
            var chest = new GameChest();
            var item = new GameItem(1, "Test Item 1");
            chest.AddOrUpdateLootTableItem(item, 10);
            //Act
            chest.DeleteIfExistsLootTableItem(new GameItem(2, "Test Item 2"));

            //Assert
            Assert.AreEqual(1, chest.LootTable.Count());
            Assert.AreEqual(item, chest.LootTable.First().Key);
        }

        [TestMethod]
        public void GivenAChestWithOneItem_WhenIRemoveTheItemFromLootTable_ThenLootTableIsEmpty()
        {
            //Arrange
            var chest = new GameChest();
            var item = new GameItem(1, "Test Item 1");
            chest.AddOrUpdateLootTableItem(item, 10);
            //Act
            chest.DeleteIfExistsLootTableItem(item);

            //Assert
            Assert.AreEqual(0, chest.LootTable.Count());
        }

        [TestMethod]
        public void GivenAChestWithNoItemsInLootTable_WhenIOpenTheChest_ThenNoItemIsGiven()
        {
            //Arrange
            var chest = new GameChest();

            //Act
            var item = chest.Open();

            //Assert
            Assert.IsNull(item);
        }

        [TestMethod]
        public void GivenAChestWithOneItemWithDropChanceZero_WhenIOpenTheChest_ThenNoItemIsGiven()
        {
            //Arrange
            var itemSword = new GameItem(1, "Sword");

            var chest = new GameChest();

            chest.AddOrUpdateLootTableItem(itemSword, 0);

            //Act
            var item = chest.Open();
            //Assert

            Assert.IsNull(item);
        }

        [TestMethod]
        public void GivenAChestWithOneItemWith100DropChance_WhenIOpenTheChest_ThenItemIsAlwaysGiven()
        {
            //Arrange
            var itemSword = new GameItem(1, "Sword");

            var chest = new GameChest();

            chest.AddOrUpdateLootTableItem(itemSword, 100);

            for (var i = 0; i < 100; i++)
            {
                //Act
                var item = chest.Open();
                //Assert

                Assert.IsNotNull(item);
                Assert.AreEqual(itemSword, item);
            }
        }

        [TestMethod]
        public void GivenAChestWithMultipleItems_WhenIOpenTheChest_ThenItemIsBasedOnDistribution()
        {
            //Arrange
            var chest = new GameChest();

            chest.AddOrUpdateLootTableItem(new GameItem(1, "Sword"), 11U);
            chest.AddOrUpdateLootTableItem(new GameItem(2, "Shield"), 8U);
            chest.AddOrUpdateLootTableItem(new GameItem(3, "Health Potion"), 27U);
            chest.AddOrUpdateLootTableItem(new GameItem(4, "Resurrection Phial"), 33U);
            chest.AddOrUpdateLootTableItem(new GameItem(5, "Scroll of wisdom"), 21U);

            var sampleSize = 10000;
            var itemFrequency = new Dictionary<GameItem, uint>();
            foreach (var item in chest.LootTable) itemFrequency[item.Key] = 0U;
            //Act

            for (var i = 0; i < sampleSize; i++) itemFrequency[chest.Open()]++;

            //Assert
            foreach (var item in chest.LootTable) 
                Assert.AreEqual(item.Value, 
                          (uint)Math.Round(itemFrequency[item.Key] * 100d / sampleSize, 0, MidpointRounding.AwayFromZero));
        }

        [TestMethod]
        public void GivenAChestWithMultipleItemsNotFullCoverage_WhenIOpenTheChest_ThenItemIsBasedOnDistribution()
        {
            //Arrange
            var chest = new GameChest();

            //drop chance sum of 79
            chest.AddOrUpdateLootTableItem(new GameItem(1, "Sword"), 11U);
            chest.AddOrUpdateLootTableItem(new GameItem(2, "Shield"), 8U);
            chest.AddOrUpdateLootTableItem(new GameItem(3, "Health Potion"), 27U);
            chest.AddOrUpdateLootTableItem(new GameItem(4, "Resurrection Phial"), 33U);


            var sampleSize = 10000;
            var itemFrequency = new Dictionary<GameItem, uint>();
            foreach (var item in chest.LootTable) itemFrequency[item.Key] = 0U;
            var emptyChestFrequency = 0;
            //Act

            for (var i = 0; i < sampleSize; i++) {
                var item = chest.Open();
                if(item != null) itemFrequency[item]++; 
                else emptyChestFrequency++;
            } 

            //Assert
            Assert.AreEqual(100 - chest.LootTable.Sum(i => i.Value),
                           (uint)Math.Round(emptyChestFrequency * 100d / sampleSize, 0, MidpointRounding.AwayFromZero));

            foreach (var item in chest.LootTable) 
                Assert.AreEqual(item.Value,
                          (uint)Math.Round(itemFrequency[item.Key] * 100d / sampleSize, 0, MidpointRounding.AwayFromZero));
        }

        [TestMethod]
        public void GivenAChestWithTwoItemConfigured100DropChance_WhenIOpenTheChest_ThenItemIsGivenOrNot2()
        {
            //Arrange
            var itemSword = new GameItem(1, "Sword");
            var itemShield = new GameItem(2, "Shield");
            var itemHealthPotion = new GameItem(3, "Health Potion");
            var itemResurrectionPhial = new GameItem(4, "Resurrection Phial");
            var itemScrollOfWisdom = new GameItem(5, "Scroll of wisdom");

            var chest = new GameChest();

            chest.AddOrUpdateLootTableItem(itemSword, 10);
            chest.AddOrUpdateLootTableItem(itemShield, 10);
            chest.AddOrUpdateLootTableItem(itemHealthPotion, 30);
            chest.AddOrUpdateLootTableItem(itemResurrectionPhial, 30);
            chest.AddOrUpdateLootTableItem(itemScrollOfWisdom, 20);

            //Act
            var item = chest.Open();
            //Assert

            Assert.IsNotNull(item);
        }
    }
}
