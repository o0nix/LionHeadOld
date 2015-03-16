using LionHead.Core.Entities;
using LionHead.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LionHead.WebAPI.Controllers
{
    public class ChestController : ApiController
    {
        private IGameAPI _gameAPI;

        public ChestController(IGameAPI gameRepository)
        {
            _gameAPI = gameRepository;
        }

        [HttpGet]
        public GameItem Loot(int playerId, int chestId)
        {
            var player = _gameAPI.GetPlayerById(playerId);
            var chest = _gameAPI.GetChestById(chestId);

            var item = chest.Open();

            if (item != null) _gameAPI.LogMessage(string.Format("{0} found a {1}", player.PlayerName, item.ItemName));
            else _gameAPI.LogMessage(string.Format("{0} found an empty chest", player.PlayerName));

            return item;
        }
    }
}
