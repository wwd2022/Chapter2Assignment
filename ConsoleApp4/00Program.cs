﻿using System.ComponentModel.Design;
using System.Numerics;
using Newtonsoft.Json;
using System.IO;

namespace ConsoleApp4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            while(true) 
            {
                // 메인메뉴 반영
                game.Start();
            }
        }
    }

    internal class Game
    {
        public Player player = new Player();
        public Shop shop = new Shop();
        // 메인메뉴 출력
        public void Start()
        {
            Console.Clear();
            Console.WriteLine(@"스파르타 마을에 오신 여러분 환영합니다.
이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.

1. 상태 보기
2. 인벤토리
3. 상점
4. 던전입장
5. 휴식하기
6. 저장하기
7. 불러오기

원하시는 행동을 입력해주세요.");

            string input = Console.ReadLine();
            switch (input)
            {
                case "1": // 상태 보기
                    while (PageOpenState()) { }
                    break;
                case "2": // 인벤토리
                    while (PageOpenInventory()) { }
                    break;
                case "3": // 상점
                    while (PageOpenShop()) { }
                    break;
                case "4": // 던전입장
                    while (PageOpenDungeon()) { }
                    break;
                case "5": // 휴식하기
                    while (PageOpenRest()) { }
                    break;
                case "6": // 데이터 저장하기
                    SaveAllItemsToJson(player, shop, "SaveData");
                    break;
                case "7": // 데이터 불러오기
                    LoadAllItemsFromJson("SaveData");
                    break;
                default:
                    Console.WriteLine("\r\n잘못된 입력입니다.");
                    Console.ReadLine();
                    break;
            }
        }

        bool PageOpenState()
        {
            double atk = player.PulsState.ATK;
            double def = player.PulsState.DEF;
            int hp = player.PulsState.HP;

            Console.Clear();
            Console.WriteLine($@"상태 보기
캐릭터의 정보가 표시됩니다.

Lv. {player.Level:D2}      
{player.Name} ( {player.Job} )
공격력 : {player.ATK} {(atk != 0 ? $"({(atk > 0 ? "+" : "")}{atk})" : "")}
방어력 : {player.DEF} {(def != 0 ? $"({(def > 0 ? "+" : "")}{def})" : "")}
체 력 : {player.HP} {(hp != 0 ? $"({(hp > 0 ? "+" : "")}{hp})" : "")}
Gold : {player.Gold} G

0. 나가기

원하시는 행동을 입력해주세요.");

            if (Console.ReadLine() == "0")
            {
                return false;
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
                Console.ReadLine();
            }
            return true;
        }

        bool PageOpenInventory()
        {
            Console.Clear();
            Console.WriteLine(@"인벤토리
보유 중인 아이템을 관리할 수 있습니다.

[아이템 목록]");

            for (int i = 0; i < player.ItemInventory.Length; i++)
            {
                var item = player.ItemInventory[i];
                if (item == null) continue;
                Console.WriteLine("- " + item.ToInventoryString());
            }
            Console.WriteLine(@"
1. 장착 관리
0. 나가기

원하시는 행동을 입력해주세요.");

            string input = Console.ReadLine();
            if (input == "1")
            {
                while (PageOpenEquipInventory()) { }
            }
            else if (input == "0")
            {
                return false;
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다");
                Console.ReadLine();
            }
            return true;
        }

        bool PageOpenEquipInventory()
        {

            Console.Clear();
            Console.WriteLine(@"인벤토리 - 장착 관리
보유 중인 아이템을 관리할 수 있습니다.

[아이템 목록]");

            for (int i = 0; i < player.ItemInventory.Length; i++)
            {
                var item = player.ItemInventory[i];
                if (item == null) { continue; }
                Console.WriteLine($"- {i + 1} " + item.ToInventoryString());
            }
            Console.WriteLine(@"
0. 나가기

원하시는 행동을 입력해주세요.");

            string input = Console.ReadLine();
            int inputNum;
            if (int.TryParse(input, out inputNum) 
                && inputNum > 0
                && inputNum <= player.ItemInventory.Length
                && player.ItemInventory[inputNum - 1] != null) 
            {
                var item = player.ItemInventory[inputNum - 1];
                int equipSlot = item.EquipSlot;
                // 장착한 같은 유형의 아이템이 없을경우 장착한다
                if (Array.FindIndex(player.ItemInventory, e => // 플레이어 인벤토리 배열에서
                    e != null  // null값이 아니며(빈칸이 아니며)
                    && e.EquipSlot == equipSlot  // 장착유형(무기, 방어구)이 같은데
                    && e.IsEquip // 착용하고 있는
                    && e != item // 선택한 장비가 아닌 다른장비가
                ) != -1) // 있다면
                {
                    Console.WriteLine($"이미 장착한 {(equipSlot == 1 ? "무기" : "방어구")}가 있습니다.");
                }
                else
                {   // 아니라면 착용또는 착용해제
                    item.IsEquip = !item.IsEquip;
                    player.UpdateState();
                    Console.WriteLine($"\"{item.Name}\" 아이템을 장착{(item.IsEquip ? "" : "해제")}했습니다");
                }
                Console.ReadLine();
            }
            else if (input == "0")
            {
                return false;
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
                Console.ReadLine();
            }
            return true;
        }
        
        bool PageOpenShop()
        {
            Console.Clear();
            Console.WriteLine($@"상점
필요한 아이템을 얻을 수 있는 상점입니다.

[보유 골드]
{player.Gold} G

[아이템 목록]");

            for (int i = 0; i < shop.Items.Length; i++)
            {
                var item = shop.Items[i];
                Console.WriteLine("- " + item.ToShopString());
            }
            Console.WriteLine(@"
1. 아이템 구매
2. 아이템 판매
0. 나가기

원하시는 행동을 입력해주세요.");

            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    while (PageOpenBuyShop()) { }
                    break;
                case "2":
                    while (PageOpenSaleShop()) { }
                    break;
                case "0":
                    return false;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.ReadLine();
                    break;
            }
            return true;
        }

        bool PageOpenBuyShop()
        {
            Console.Clear();
            Console.WriteLine($@"상점 - 아이템 구매
필요한 아이템을 얻을 수 있는 상점입니다.

[보유 골드]
{player.Gold} G

[아이템 목록]");

            for (int i = 0; i < shop.Items.Length; i++)
            {
                var item = shop.Items[i];
                Console.WriteLine($"- {i + 1} " + item.ToShopString());
            }
            Console.WriteLine(@"
0. 나가기

원하시는 행동을 입력해주세요.");

            string input = Console.ReadLine();
            int inputNum;
            if (int.TryParse(input, out inputNum) && inputNum > 0 && inputNum <= shop.Items.Length)
            {
                PlayerItem item = shop.Items[inputNum - 1];
                    
                if (player.Gold >= item.Gold)
                {
                    item.IsBuy = true;
                    player.Gold -= item.Gold;
                    player.AddItemInventory(item);
                    Console.WriteLine($"\"{item.Name}\" 아이템을 구매하였습니다.");
                    Console.WriteLine($"남은 소지금은 {player.Gold} G 입니다.");
                }
                else if (item.IsBuy)
                {
                    Console.WriteLine("이미구매한 아이템입니다.");
                }
                else if (player.Gold < item.Gold)
                {
                    Console.WriteLine("Gold 가 부족합니다.");
                }
                Console.ReadLine();

            }
            else if (input == "0")
            {
                return false;
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
                Console.ReadLine();
            }
            return true;
        }

        bool PageOpenSaleShop()
        {
            Console.Clear();
            Console.WriteLine($@"상점 - 아이템 판매
필요한 아이템을 얻을 수 있는 상점입니다.

[보유 골드]
{player.Gold} G

[아이템 목록]");
            for (int i = 0; i < player.ItemInventory.Length; i++)
            {
                var item = player.ItemInventory[i];
                if (item == null) { continue; }
                Console.WriteLine($"- {i + 1} " + item.ToInventoryString() + $" | {(int)(item.Gold * 0.85)}");
            }
            Console.WriteLine(@"
0. 나가기

원하시는 행동을 입력해주세요.");
            string input = Console.ReadLine();
            int inputNum;
            if (int.TryParse(input, out inputNum) 
                && inputNum > 0 
                && inputNum <= player.ItemInventory.Length
                && player.ItemInventory[inputNum - 1] != null)
            {
                // 아이템 이름을 기준으로 아이템을 탐색하고 판매한다.
                var item = player.ItemInventory[inputNum - 1];
                string name = item.Name.ToString();
                int index = Array.FindIndex(shop.Items, i => i.Name == item.Name);
                shop.Items[index].IsBuy = false;
                int gold = (int)(item.Gold * 0.85);
                player.Gold += gold;
                player.DelItemInventory(item);
                player.UpdateState();
                Console.WriteLine($"\"{name}\" 아이템을 판매했습니다");
                Console.WriteLine($"소지금이 {gold} G 증가하였습니다");
                Console.ReadLine();
            }
            else if (input == "0")
            {
                return false;
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
                Console.ReadLine();
            }
            return true;
        }

        bool PageOpenDungeon()
        {
            Console.Clear();
            Console.WriteLine(@"던전입장
이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.

1. 쉬운 던전     | 방어력 5 이상 권장
2. 일반 던전     | 방어력 11 이상 권장
3. 어려운 던전    | 방어력 17 이상 권장
0. 나가기

원하시는 행동을 입력해주세요.");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    PageOpenStartDungeon("쉬운 던전", 5, 1000);
                    break;
                case "2":
                    PageOpenStartDungeon("일반 던전", 11, 1700);
                    break;
                case "3":
                    PageOpenStartDungeon("어려운 던전", 17, 2500);
                    break;
                case "0":
                    return false;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.ReadLine();
                    break;
            }
            return true;
        }

        /// <summary>
        /// 던전 입장
        /// </summary>
        /// <param name="dungeonDef"> 권장방어력 </param>
        /// <param name="dungeonGold"> 클리어 보상 금액 </param>
        /// <returns></returns>
        void PageOpenStartDungeon(string dungeonName, float dungeonDef, int dungeonGold)
        {
            Console.Clear();
            if (player.HP <= 0)
            {
                while (true)
                {
                    Console.WriteLine(@"던전 입장불가
체력이 없습니다.
휴식으로 체력을 회복해주세요.

0. 나가기

원하시는 행동을 입력해주세요.");
                    if (Console.ReadLine() == "0")
                    {
                        return; ;
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Console.ReadLine();
                    }
                }
            }
            bool isDungeonClear = false;
            int dmg = 0;
            int gold = 0;
            Random rand = new Random();
            if (player.DEF + player.PulsState.DEF < dungeonDef && rand.Next(0, 10) < 4)
            {
                isDungeonClear = false;
                dmg = (int)(player.HP / 2);
            }
            else
            {
                isDungeonClear = true;
                int dmgMin = 25 + Convert.ToInt32(dungeonDef - player.DEF + player.PulsState.DEF);
                int dmgMax = 36 + Convert.ToInt32(dungeonDef - player.DEF + player.PulsState.DEF);
                dmg = rand.Next(dmgMin, dmgMax);
                int goldMin = Convert.ToInt32(player.ATK + player.PulsState.ATK);
                int goldMax = Convert.ToInt32((player.ATK + player.PulsState.ATK) * 2) + 1;
                float pulsGoldFloat = rand.Next(goldMin, goldMax) / 100;
                gold = dungeonGold + (int)(dungeonGold * pulsGoldFloat);
            }

            if (dmg < 0)
            {
                dmg = 0;
            }
            if (player.HP < dmg)
            {
                dmg = player.HP;
            }
            player.HP -= dmg;
            player.Gold += gold;
            player.Exp += 1;
            bool isLevelUp = player.LevelUpCheck();
            while (true)
            {
                Console.Clear();
                if (isDungeonClear)
                {
                    Console.WriteLine($@"던전 클리어
축하합니다!!
{dungeonName}을 클리어 하였습니다.");
                }
                else
                {
                    Console.WriteLine($@"던전 클리어 실패
{dungeonName}을 클리어하지 못했습니다.");
                }

                Console.WriteLine($@"
[탐험 결과]
체력 {player.HP + dmg} -> {player.HP}
Gold {player.Gold - gold} G -> {player.Gold} G");

                if (isLevelUp)
                {
                    Console.WriteLine("레벨이 1 상승했습니다.");
                    Console.WriteLine("공격력 0.5, 방어력 1이 증가합니다.");
                }
                Console.WriteLine(@"
0. 나가기

원하시는 행동을 입력해주세요.");

                if (Console.ReadLine() == "0")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.ReadLine();
                }
            }   
        }

        bool PageOpenRest()
        {
            Console.Clear();
            Console.WriteLine($@"휴식하기
500 G 를 내면 체력을 회복할 수 있습니다. (보유 골드 : {player.Gold} G)

1. 휴식하기
0. 나가기

원하시는 행동을 입력해주세요.");
            string input = Console.ReadLine();
            if (input == "1")
            {
                if (player.HP < 100)
                {
                    if (player.Gold >= 500)
                    {
                        player.Gold -= 500;
                        player.HP = 100;
                        Console.WriteLine("체력이 회복되었습니다.");
                    }
                    else
                    {
                        Console.WriteLine("골드가 부족합니다.");
                    }
                }
                else
                {
                    Console.WriteLine("더 회복할 수 없습니다.");
                }
            }
            else if (input == "0")
            {
                return false;
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
            }
            Console.ReadLine();
            return true;
        }
        
        static void SaveAllItemsToJson(Player player, Shop shop, string filePath)
        {
            string jsonPlayer = JsonConvert.SerializeObject(player, Formatting.Indented);
            File.WriteAllText(filePath + "Player", jsonPlayer); // Json 문자열을 파일로 저장
            string jsonShop = JsonConvert.SerializeObject(shop, Formatting.Indented);
            File.WriteAllText(filePath + "Shop", jsonShop); // Json 문자열을 파일로 저장
            Console.WriteLine("데이터가 저장되었습니다");
            Console.ReadLine();
        }

        void LoadAllItemsFromJson(string filePath)
        {
            // 파일로부터 JSON 문자열을 읽기
            string jsonPlayer = File.ReadAllText(filePath + "Player");
            // Json 문자열로부터 아이템 리스트를 역직렬화
            Player? playerData = JsonConvert.DeserializeObject<Player>(jsonPlayer);
            if (playerData != null)
            {
                player = playerData;
                Console.WriteLine("상태 데이터를 불러왔습니다.");
            }
            else
            {
                Console.WriteLine("상태 데이터를 찾지못했습니다.");
            }
            // 파일로부터 JSON 문자열을 읽기
            string jsonShop = File.ReadAllText(filePath + "Shop");
            // Json 문자열로부터 아이템 리스트를 역직렬화
            Shop? shopData = JsonConvert.DeserializeObject<Shop>(jsonShop);
            if (shopData != null) 
            {
                shop = shopData;
                Console.WriteLine("상점 데이터를 불러왔습니다.");
            }
            else
            {
                Console.WriteLine("상점 데이터를 찾지못했습니다.");
            }
            Console.ReadLine();
        }
    }
}
