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
        /// <summary> 구매여부 </summary>
        public bool IsBuy;
        /// <summary> 장착여부 </summary>
        public bool Equip;
        /// <summary> 1:무기 2:방어구 </summary>
        public int EquipSlot;
        /// <summary> 아이템설명 </summary>
        public string Explication;
        public PlayerItem()
        {
            IsBuy = false;
            Equip = false;
            Explication = string.Empty;
        }

        public PlayerItem(string[] arr)
        {
            IsBuy = false;
            Equip = false;
            Name = arr[0];
            //Job;
            //Level;
            ATK = int.Parse(arr[1]);
            DEF = int.Parse(arr[2]);
            HP = int.Parse(arr[3]);
            EquipSlot = int.Parse(arr[4]);
            Gold = int.Parse(arr[5]);
            Explication = arr[6];
        }
    }
}
