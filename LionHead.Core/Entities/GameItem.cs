using LionHead.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LionHead.Core.Entities
{
    public class GameItem
    {
        public int Id { get; set; }
        public string ItemName {get; private set;}

        public GameItem(int id, string itemName)
        {
            this.Id = id;
            this.ItemName = itemName;
        }
    }
}
