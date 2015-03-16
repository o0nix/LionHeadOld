using LionHead.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LionHead.Core.Entities
{
    public class GamePlayer
    {
        public int Id { get; set; }

        public string PlayerName { get; set; }

        public GamePlayer(int playerId, string playerName)
        {
            Id = playerId;
            PlayerName = playerName;
        }

        public void Loot(GameChest chest)
        {
            throw new NotImplementedException();
        }
    }
}
