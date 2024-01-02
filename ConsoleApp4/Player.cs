using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    class DefaultState
    {
        public string Name;
        public string Job;
        public int Level;
        public int ATK;
        public int DEF;
        public int HP;
        public int Gold;

        public DefaultState()
        {
            Name = string.Empty;
            Job = string.Empty;
        }
    }

    class Player : DefaultState
    {
        public PlayerItem[] ItemInventory;

        // 플레이어 생성시 기본 능력치
        public Player()
        {
            Name = "익명";
            Job = "전사";
            Level = 1;
            ATK = 10;
            DEF = 5;
            HP = 100;
            Gold = 1500;
            ItemInventory = new PlayerItem[100];
        }
    }

    class PlayerItem : DefaultState
    {
    }
}
