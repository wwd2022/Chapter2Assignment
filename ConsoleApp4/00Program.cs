using System.ComponentModel.Design;
using System.Numerics;

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
        Player player = new Player();
        Shop shop = new Shop();
        // 메인메뉴 출력
        public void Start()
        {
            Console.Clear();
            Console.WriteLine(@"스파르타 마을에 오신 여러분 환영합니다.
이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.

1. 상태 보기
2. 인벤토리
3. 상점

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
                default:
                    Console.WriteLine("\r\n잘못된 입력입니다.");
                    Console.ReadLine();
                    break;
            }
        }

        bool PageOpenState()
        {
            int atk = player.PulsState.ATK;
            int def = player.PulsState.DEF;
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
                item.IsEquip = !item.IsEquip;
                player.UpdateState();
                Console.WriteLine($"\"{item.Name}\" 아이템을 장착{(item.IsEquip ? "" : "해제")}했습니다");
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
                    
                if (player.Gold > item.Gold)
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
    }
}
