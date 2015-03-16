using LionHead;
using LionHead.Core.Entities;
using LionHead.Core.Interfaces;
using LionHead.WebAPI;
using Microsoft.Owin.Hosting;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Specs
{
    [Binding]
    public class ChestItemsSteps
    {
        private const string GameAPIMockKey = "GameAPIMock";
        private const string PlayerKey = "Player";
        private const string ChestKey = "Chest";
        private const string ItemKey = "Item";

        private static string _baseUrl = "http://localhost:9999/";

        private static IDisposable _service;

        [BeforeFeature]
        public static void InitializeFeatureTest()
        {
            _service = WebApp.Start<Startup>(url: _baseUrl);
        }

        [BeforeScenario]
        public static void InitializeScenarioTest() 
        {
            var gameAPIMock = new Mock<IGameAPI>();
            UnityConfig.RegisteredTypes.RegisterInstance<IGameAPI>(gameAPIMock.Object);
            ScenarioContext.Current.Set<Mock<IGameAPI>>(gameAPIMock, GameAPIMockKey);
        }

        [AfterFeature]
        public static void CleanUp()
        {
            _service.Dispose();
        }

        [Given(@"I have a player")]
        public static void GivenIHaveAPlayer()
        {
            var player = new GamePlayer(1, "BDD Game Player");
            var gameAPIMock = ScenarioContext.Current.Get<Mock<IGameAPI>>(GameAPIMockKey);
            gameAPIMock.Setup(x => x.GetPlayerById(It.IsAny<int>())).Returns(player);
            ScenarioContext.Current.Set<GamePlayer>(player, PlayerKey);
        }

        [Given(@"a configured loot table:")]
        public static void GivenAConfiguredLootTable(Table table)
        {
            var chest = new GameChest();
            chest.Id = 1;
            var index = 1;
            foreach (var row in table.Rows)
            {
                chest.AddOrUpdateLootTableItem(new GameItem(index, row[0]), uint.Parse(row[1]));
                index++;
            }
            var gameAPIMock = ScenarioContext.Current.Get<Mock<IGameAPI>>(GameAPIMockKey);
            gameAPIMock.Setup(x => x.GetChestById(It.IsAny<int>())).Returns(chest);

            ScenarioContext.Current.Set<GameChest>(chest, ChestKey);
        }

        [When(@"I roll on this loot table")]
        public static void WhenIRollOnThisLootTable()
        {
            using(var client = new HttpClient())
            {
                var player = ScenarioContext.Current.Get<GamePlayer>(PlayerKey);
                var chest = ScenarioContext.Current.Get<GameChest>(ChestKey);

                var resp = client.GetAsync(string.Format("{0}api/chest?playerId={1}&chestId={2}", ChestItemsSteps._baseUrl, 
                                                                                                    player.Id, 
                                                                                                    chest.Id)).Result;

                var item = resp.Content.ReadAsAsync<GameItem>().Result;

                if (item != null) ScenarioContext.Current.Set<GameItem>(item, ItemKey);
            }
        }

        [Then(@"I receive a random item from the loot table")]
        public static void ThenIReceiveARandomItemFromTheLootTable()
        {
            var item = ScenarioContext.Current.Get<GameItem>(ItemKey);
            var chest = ScenarioContext.Current.Get<GameChest>(ChestKey);

            Assert.IsNotNull(item);
            Assert.IsNotNull(chest.LootTable.FirstOrDefault(x => x.Key.Id == item.Id && x.Key.ItemName == item.ItemName));
        }

        [Then(@"I receive a sword from the loot table")]
        public void ThenIReceiveASwordFromTheLootTable()
        {
            var item = ScenarioContext.Current.Get<GameItem>(ItemKey);
            Assert.IsNotNull(item);
            Assert.AreEqual(1, item.Id);
            Assert.AreEqual("Sword", item.ItemName);
        }

        [Then(@"the chest is empty")]
        public void ThenTheChestIsEmpty()
        {
            Assert.IsFalse(ScenarioContext.Current.ContainsKey(ItemKey));
        }

        [Then(@"a log is written with the players username and received item")]
        public static void ThenALogIsWrittenWithThePlayersUsernameAndReceivedItem()
        {
            var player = ScenarioContext.Current.Get<GamePlayer>(PlayerKey);
            var item = ScenarioContext.Current.Get<GameItem>(ItemKey);

            var gameAPIMock = ScenarioContext.Current.Get<Mock<IGameAPI>>(GameAPIMockKey);
            gameAPIMock.Verify(x => x.LogMessage(string.Format("{0} found a {1}", player.PlayerName, item.ItemName)), Times.Once);
        }

        [Then(@"a log is written with the players username and that the chest was empty")]
        public void ThenALogIsWrittenWithThePlayersUsernameAndThatTheChestWasEmpty()
        {
            var player = ScenarioContext.Current.Get<GamePlayer>(PlayerKey);

            var gameAPIMock = ScenarioContext.Current.Get<Mock<IGameAPI>>(GameAPIMockKey);
            gameAPIMock.Verify(x => x.LogMessage(string.Format("{0} found an empty chest", player.PlayerName)), Times.Once);
        }

    }
}

