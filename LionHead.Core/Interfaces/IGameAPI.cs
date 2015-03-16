using LionHead.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LionHead.Core.Interfaces
{
    public interface IGameAPI
    {
        GameChest GetChestById(int chestId);
        GamePlayer GetPlayerById(int playerId);
        void LogMessage(string Message);
    }
}
