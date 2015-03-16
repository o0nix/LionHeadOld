using LionHead.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LionHead.Core.Entities
{
    public class GameChest
    {
        public int Id { get; set; }

        public Dictionary<GameItem, uint> LootTable { get; private set; }

        private Random _numberGenerator;

        private int _emptyChestChance;

        public GameChest()
        {
            _numberGenerator = new Random(0);
            _emptyChestChance = 100;
            LootTable = new Dictionary<GameItem, uint>();
        }

        public GameItem Open()
        {
            GameItem result = null;
            var randomNumber = _numberGenerator.Next(1, 101);
            var temp = 0U;
            foreach (var item in LootTable.OrderByDescending(i => i.Key.Id))
            {
                if (randomNumber <= (temp + item.Value))
                {
                    result = item.Key;
                    break;
                }
                temp += item.Value;
            }
            return result;
        }

        public void AddOrUpdateLootTableItem(GameItem item, uint dropChance)
        {
            var existingItemDropChance = 0;

            if (LootTable.ContainsKey(item)) existingItemDropChance = (int)LootTable[item];

            if (_emptyChestChance + existingItemDropChance - dropChance >= 0)
            {
                LootTable[item] = dropChance;
                _emptyChestChance -= (int)dropChance;
            }
            else throw new InvalidOperationException(string.Format("Max drop chance allowed to be added is {0}.", _emptyChestChance));
        }

        public void DeleteIfExistsLootTableItem(GameItem itemSword)
        {
            if (LootTable.ContainsKey(itemSword)) LootTable.Remove(itemSword);
        }
    }
}
