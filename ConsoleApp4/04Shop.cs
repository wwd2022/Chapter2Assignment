using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    class Shop
    {
        public PlayerItem[] Items;
        public Shop()
        { 
            // 이름,공격력,방어력,체력,착용 슬롯(1무기, 2방어구),가격,설명
            string[] dataArr = @"낡은 갑옷,0,3,0,2,500,쉽게 볼 수 있는 낡은 갑옷이다.
낡은 검,3,0,0,1,500,쉽게 볼 수 있는 낡은 검이다.
청동 검,5,0,0,1,700,청동으로 만들어진 검이다.
청동 도끼,7,0,0,1,800,청동으로 만들어진 도끼다.
무쇠갑옷,0,7,0,2,1000,무쇠로 만들어져 튼튼한 갑옷입니다.
무쇠검,7,0,0,1,1000,무쇠로 만들어져 튼튼한 검입니다.
수련자 갑옷,0,9,0,2,1200,수련에 도움을 주는 갑옷입니다.
수련자 검,9,0,0,1,1200,수련에 도움을 주는 검입니다.
스파르타의 갑옷,0,15,0,2,1500,스파르타의 전사들이 사용했다는 전설의 갑옷입니다.
스파르타의 검,10,0,0,1,1500,스파르타의 전사들이 사용했다는 전설의 검입니다.
스파르타의 창,12,0,0,1,1800,스파르타의 전사들이 사용했다는 전설의 창입니다.".Split(Environment.NewLine);
            Items = new PlayerItem[dataArr.Length];
            for (int i = 0; i < dataArr.Length; i++)
            {
                string[] itemData = dataArr[i].Split(',');
                if (itemData.Length != 7) { Console.WriteLine($"{i + 1}번째 아이템데이터 의도된 양식아님"); continue; }
                Items[i] = new PlayerItem(itemData);
            }
        }
    }
}
