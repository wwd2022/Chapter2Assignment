using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    class PlayerItem : DefaultState
    {
        /// <summary> 구매여부 </summary>
        public bool IsBuy;
        /// <summary> 장착여부 </summary>
        public bool IsEquip;
        /// <summary> 1:무기 2:방어구 </summary>
        public int EquipSlot;
        /// <summary> 아이템설명 </summary>
        public string Explication;
        public PlayerItem()
        {
            IsBuy = false;
            IsEquip = false;
            Explication = string.Empty;
        }

        public PlayerItem(string[] arr)
        {
            IsBuy = Convert.ToBoolean(int.Parse(arr[7]));
            IsEquip = Convert.ToBoolean(int.Parse(arr[8]));
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

        public string ToInventoryString()
        {
            string str1 = (IsEquip ? "[E]" : "") + Name;
            string str2 = "";
            if (ATK != 0) str2 += $" 공격력 {(ATK > 0 ? "+" : "-")}{ATK}";
            if (DEF != 0) str2 += $" 방어력 {(DEF > 0 ? "+" : "-")}{DEF}";
            if (HP != 0) str2 += $" 체력 {(HP > 0 ? "+" : "-")}{HP}";
            string str3 = Explication;
            return $"{str1,-10} |{str2} | {str3,-30}";
        }

        public string ToShopString()
        {

            string str1 = Name;
            string str2 = "";
            if (ATK != 0) str2 += $" 공격력 {(ATK > 0 ? "+" : "-")}{ATK}";
            if (DEF != 0) str2 += $" 방어력 {(DEF > 0 ? "+" : "-")}{DEF}";
            if (HP != 0) str2 += $" 체력 {(HP > 0 ? "+" : "-")}{HP}";
            string str3 = Explication;
            return $"{str1,-10} |{str2} | {str3,-30} | {(IsBuy ? "구매완료" : $"{Gold} G")}";
        }
    }
}
