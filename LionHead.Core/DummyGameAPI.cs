using LionHead.Core.Entities;
using LionHead.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LionHead.Core
{
    public class DummyGameAPI : IGameAPI
    {
        GameChest _chest;

        public DummyGameAPI()
        {
            _chest = new GameChest();
            _chest.AddOrUpdateLootTableItem(new GameItem(1, "Sword"), 11U);
            _chest.AddOrUpdateLootTableItem(new GameItem(2, "Shield"), 8U);
            _chest.AddOrUpdateLootTableItem(new GameItem(3, "Health Potion"), 27U);
            _chest.AddOrUpdateLootTableItem(new GameItem(4, "Resurrection Phial"), 33U);
            _chest.AddOrUpdateLootTableItem(new GameItem(5, "Scroll of wisdom"), 21U);
        }

        public GameChest GetChestById(int chestId)
        {                      
            return _chest;
        }

        public GamePlayer GetPlayerById(int playerId)
        {
            return new GamePlayer(playerId, "Dummy Player");
        }

        public void LogMessage(string Message)
        {
            //Do nothing
        }
    }
}
