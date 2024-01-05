using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    class Player : DefaultState
    {
        /// <summary> 인벤토리 </summary>
        public PlayerItem?[] ItemInventory;
        /// <summary> 아이템으로 증가한 스텟 </summary>
        public DefaultState PulsState;

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
            PulsState = new DefaultState();
        }

        /// <summary>
        /// 인벤토리에 아이템추가 성공시 true 실패시 false
        /// </summary>
        public bool AddItemInventory(PlayerItem item)
        {
            int index = Array.IndexOf(ItemInventory,null);
            if (index == -1) return false;
            item.IsEquip = false;
            ItemInventory[index] = item;
            return true;
        }

        public bool DelItemInventory(int index)
        {
            if (ItemInventory[index] == null) return false;
            ItemInventory[index] = null;
            int len = ItemInventory.Length - 1;
            for (int i = index; i < len; i++)
            {
                if (ItemInventory[i + 1] != null && ItemInventory[i] == null)
                {
                    ItemInventory[i] = ItemInventory[i + 1];
                    ItemInventory[i + 1] = null;
                }
            }
            return true;
        }

        public bool DelItemInventory(PlayerItem item)
        {
            int index = Array.IndexOf(ItemInventory, item);
            if (index == -1) return false;
            ItemInventory[index] = null;
            int len = ItemInventory.Length - 1;
            for (int i = index; i < len; i++)
            {
                if (ItemInventory[i + 1] != null && ItemInventory[i] == null)
                {
                    ItemInventory[i] = ItemInventory[i + 1];
                    ItemInventory[i + 1] = null;
                }
            }
            return true;
        }

        public void UpdateState()
        {
            PulsState = new DefaultState();
            for (int i = 0; i < ItemInventory.Length; i++)
            {
                var item = ItemInventory[i];
                if (item == null || !item.IsEquip) continue;
                PulsState.ATK += item.ATK;
                PulsState.DEF += item.DEF;
                PulsState.HP += item.HP;
            }
        }
    }
}
