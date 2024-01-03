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
                game.Start();
            }
        }
    }

    internal class Game
    {
        Player player = new Player();
        public void Start()
        {
            Console.WriteLine(@"
스파르타 마을에 오신 여러분 환영합니다.
이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.

1. 상태 보기
2. 인벤토리
3. 상점

원하시는 행동을 입력해주세요.");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    ShowPage("상태 보기");
                    break;
                case "2":
                    ShowPage("인벤토리");
                    break;
                case "3":
                    ShowPage("상점");
                    break;
                default:
                    Console.WriteLine("\r\n잘못된 입력입니다.");
                    Console.ReadLine();
                    break;
            }
        }

        bool ShowPage(string pageName)
        {
            switch (pageName)
            {
                case "상태 보기":
                    PageOpenState();
                    break;
                case "인벤토리":
                    PageOpenInventory();
                    break;
                case "상점":
                    PageOpenShop();
                    break;
                default:
                    return false;
            }
            return true;
        }

        void PageOpenState()
        {
            string input;
            do
            {
                Console.WriteLine(String.Format(@"
상태 보기
캐릭터의 정보가 표시됩니다.

Lv. {0:D2}      
{1} ( {2} )
공격력 : {3}
방어력 : {4}
체 력 : {5}
Gold : {6} G

0. 나가기

원하시는 행동을 입력해주세요.", player.Level, player.Name, player.Job, player.ATK, player.DEF, player.HP, player.Gold));
            } while (Console.ReadLine() != "0");
        }

        void PageOpenInventory()
        {
            Console.WriteLine(@"
인벤토리
보유 중인 아이템을 관리할 수 있습니다.

[아이템 목록]");
            $""
                
            // - [E]무쇠갑옷      | 방어력 +5 | 무쇠로 만들어져 튼튼한 갑옷입니다.
            // - [E]스파르타의 창  | 공격력 +7 | 스파르타의 전사들이 사용했다는 전설의ine니다.
            // - 낡은 검         | 공격력 +2 | 쉽게 볼 수 있는 낡은 ine니다.
            Console.WriteLine(@"
1. 장착 관리
2. 나가기

원하시는 행동을 입력해주세요.");
        }
        
        void PageOpenShop()
        {
            Console.WriteLine(@"상점
필요한 아이템을 얻을 수 있는 상점입니다.

[보유 골드]
800 G

[아이템 목록]
- 수련자 갑옷    | 방어력 +5  | 수련에 도움을 주는 갑옷입니다.             |  1000 G
- 무쇠갑옷      | 방어력 +9  | 무쇠로 만들어져 튼튼한 갑옷입니다.           |  구매완료
- 스파르타의 갑옷 | 방어력 +15 | 스파르타의 전사들이 사용했다는 전설의 갑옷입니다.|  3500 G
- 낡은 검      | 공격력 +2  | 쉽게 볼 수 있는 낡은 검 입니다.            |  600 G
- 청동 도끼     | 공격력 +5  |  어디선가 사용됐던거 같은 도끼입니다.        |  1500 G
- 스파르타의 창  | 공격력 +7  | 스파르타의 전사들이 사용했다는 전설의 창입니다. |  구매완료

1. 아이템 구매
0. 나가기

원하시는 행동을 입력해주세요.");
        }
    }
}
