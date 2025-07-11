// 텍스트 RPG: 스파르타 마을

using System; // 기본 입출력(Console 등)을 위한 네임스페이스
using System.Collections.Generic; // List<T> 같은 제네릭 컬렉션을 사용하기 위한 네임스페이스
using System.Linq; // LINQ 메서드(Sum, Where 등)를 사용하기 위한 네임스페이스

namespace SpartaVillageRPG // 전체 프로그램의 네임스페이스 정의
{
    class Program // 메인 클래스 정의
    {
        // ===== 캐릭터 상태 전역 변수 정의 =====
        static string name; // 플레이어 이름
        static string job; // 플레이어 직업
        static int level = 1; // 초기 레벨
        static int exp = 0; // 경험치 (기능 미구현 상태)
        static int baseAtk = 0; // 기본 공격력
        static int baseDef = 0; // 기본 방어력
        static int baseHp = 100; // 최대 체력
        static int currentHp = 100; // 현재 체력
        static int gold = 1500; // 초기 보유 골드

        // ===== 아이템 클래스 정의 =====
        class Item
        {
            public string Name; // 아이템 이름
            public string Type; // 아이템 종류 ("공격력" 또는 "방어력")
            public int Value; // 아이템의 능력치 증가 수치
            public string Desc; // 아이템 설명
            public bool Equipped; // 장착 여부
            public bool Purchased; // 상점에서 구매했는지 여부

            // 생성자: 아이템 생성 시 속성 설정
            public Item(string name, string type, int value, string desc, bool equipped = false, bool purchased = false)
            {
                Name = name; // 이름 설정
                Type = type; // 타입 설정
                Value = value; // 수치 설정
                Desc = desc; // 설명 설정
                Equipped = equipped; // 장착 여부 초기값
                Purchased = purchased; // 구매 여부 초기값
            }
        }

        // ===== 인벤토리 및 상점 아이템 목록 =====
        static List<Item> inventory = new List<Item>(); // 플레이어 인벤토리 리스트
        static List<Item> shop = new List<Item> // 상점에서 판매 중인 아이템 리스트
        {
            new Item("수련자 갑옷", "방어력", 5, "수련에 도움을 주는 갑옷입니다."), // 가격 1000G
            new Item("무쇠갑옷", "방어력", 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", true, true), // 장착 및 구매된 상태
            new Item("스파르타의 갑옷", "방어력", 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다."), // 가격 3500G
            new Item("낡은 검", "공격력", 2, "쉽게 볼 수 있는 낡은 검 입니다."), // 가격 600G
            new Item("청동 도끼", "공격력", 5, "어디선가 사용됐던거 같은 도끼입니다."), // 가격 1500G
            new Item("스파르타의 창", "공격력", 7, "스파르타의 전사들이 사용했다는 전설의 창입니다.", true, true) // 장착 및 구매된 상태
        };

        // ===== 프로그램 실행 진입점 (메인 함수) =====
        static void Main(string[] args)
        {
            StartStory(); // 배경 스토리 및 이름 입력
            SelectJob(); // 직업 선택

            // 기본 장비 지급 (무쇠갑옷과 스파르타의 창)
            inventory.Add(shop[1]); // 무쇠갑옷 인벤토리에 추가
            inventory.Add(shop[5]); // 스파르타의 창 인벤토리에 추가

            // 메인 루프: 마을 기능 선택 반복
            while (true)
            {
                Console.Clear(); // 콘솔 화면 초기화
                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n");
                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine("4. 던전 가기\n");
                Console.Write("원하시는 행동을 입력해주세요.\n>> ");

                string input = Console.ReadLine(); // 사용자 입력 받기
                switch (input) // 입력값에 따라 기능 선택
                {
                    case "1": ShowStatus(); break; // 상태 보기
                    case "2": InventoryMenu(); break; // 인벤토리 열기
                    case "3": ShopMenu(); break; // 상점 열기
                    case "4": GoToDungeon(); break; // 던전 진입
                    default: // 잘못된 입력 처리
                        Console.WriteLine("잘못된 입력입니다.");
                        Console.ReadKey(); // 아무 키나 눌러야 진행
                        break;
                }
            }
        }

        // ===== 이름과 간단한 스토리 설명 함수 =====
        static void StartStory()
        {
            Console.Clear(); // 화면 초기화
            Console.WriteLine("눈을 뜬 당신은 작은 마을의 침대 위에 누워 있었습니다.\n");
            Console.WriteLine("지나가던 할머니가 말합니다: \"용사님, 일어나셨군요! 마왕이 곧 부활한다고 해요!\"\n");
            Console.WriteLine("당신은 아직 혼란스럽지만... 뭔가 해야 할 것 같습니다.\n");

            while (true)
            {
                Console.Write("이름을 입력해주세요: "); // 이름 입력 요청
                name = Console.ReadLine(); // 이름 입력

                if (!string.IsNullOrWhiteSpace(name)) break; // 유효한 이름인지 확인
                Console.WriteLine("이름은 비워둘 수 없습니다. 다시 입력해주세요.\n");
            }
        }

        // ===== 직업 선택 함수 =====
        static void SelectJob()
        {
            Console.Clear(); // 화면 초기화
            Console.WriteLine("직업을 선택하세요:");
            Console.WriteLine("1. 전사 (공격력 10, 방어력 5)");
            Console.WriteLine("2. 마법사 (공격력 15, 방어력 2)");
            Console.WriteLine("3. 도적 (공격력 8, 방어력 4, 골드 +500)");
            Console.Write(">> ");

            string input = Console.ReadLine(); // 입력값 받기
            switch (input)
            {
                case "1": job = "전사"; baseAtk = 10; baseDef = 5; break;
                case "2": job = "마법사"; baseAtk = 15; baseDef = 2; break;
                case "3": job = "도적"; baseAtk = 8; baseDef = 4; gold += 500; break;
                default: // 잘못된 입력 시 기본값 전사
                    Console.WriteLine("잘못된 입력입니다. 전사로 시작합니다.");
                    job = "전사"; baseAtk = 10; baseDef = 5; break;
            }

            Console.WriteLine($"\n{name}님은 {job}으로 선택되었습니다!\n");
            Console.WriteLine("엔터를 눌러 계속...");
            Console.ReadLine();
        }

        // ===== 캐릭터 상태 출력 함수 =====
        static void ShowStatus()
        {
            // 장착된 장비의 능력치를 반영한 총 공격력 및 방어력 계산
            int atk = baseAtk + inventory.Where(i => i.Equipped && i.Type == "공격력").Sum(i => i.Value);
            int def = baseDef + inventory.Where(i => i.Equipped && i.Type == "방어력").Sum(i => i.Value);

            Console.Clear(); // 화면 초기화
            Console.WriteLine("[상태 보기]");
            Console.WriteLine($"Lv. {level}");
            Console.WriteLine($"{name} ({job})");
            Console.WriteLine($"공격력 : {atk}");
            Console.WriteLine($"방어력 : {def}");
            Console.WriteLine($"체력 : {currentHp} / {baseHp}");
            Console.WriteLine($"Gold : {gold} G");
            Console.WriteLine("엔터를 누르면 돌아갑니다.");
            Console.ReadLine();
        }

        // ===== 인벤토리 출력 함수 =====
        static void InventoryMenu()
        {
            Console.Clear(); // 화면 초기화
            Console.WriteLine("[인벤토리]");
            Console.WriteLine("보유 중인 아이템 목록:");

            for (int i = 0; i < inventory.Count; i++) // 인벤토리 목록 출력
            {
                var item = inventory[i];
                string equipped = item.Equipped ? "[E]" : "   "; // 장착 상태 출력
                Console.WriteLine($"{i + 1}. {equipped}{item.Name} ({item.Type} +{item.Value}) - {item.Desc}");
            }

            Console.WriteLine("엔터를 누르면 돌아갑니다.");
            Console.ReadLine();
        }

        // ===== 상점 인터페이스 함수 =====
        static void ShopMenu()
        {
            while (true) // 반복적으로 상점 이용 가능
            {
                Console.Clear(); // 화면 초기화
                Console.WriteLine("[상점]");
                Console.WriteLine($"보유 골드: {gold} G\n");

                for (int i = 0; i < shop.Count; i++) // 상점 아이템 목록 출력
                {
                    var item = shop[i];
                    string status = item.Purchased ? "구매완료" : $"{GetPrice(item)} G"; // 구매 상태 표시
                    Console.WriteLine($"{i + 1}. {item.Name} ({item.Type} +{item.Value}) - {item.Desc} | {status}");
                }

                Console.WriteLine("\n아이템 번호를 입력해 구매하거나 0을 눌러 나가세요.");
                Console.Write(">> ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int index)) // 숫자 입력 확인
                {
                    if (index == 0) break; // 나가기
                    if (index < 1 || index > shop.Count) // 유효 범위 검사
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Console.ReadKey();
                        continue;
                    }

                    var item = shop[index - 1];
                    if (item.Purchased) // 이미 구매한 경우
                    {
                        Console.WriteLine("이미 구매한 아이템입니다.");
                    }
                    else if (gold >= GetPrice(item)) // 골드 충분하면 구매
                    {
                        gold -= GetPrice(item); // 골드 차감
                        item.Purchased = true; // 구매 상태로 변경
                        inventory.Add(item); // 인벤토리에 추가
                        Console.WriteLine("구매 완료!");
                    }
                    else // 골드 부족
                    {
                        Console.WriteLine("Gold가 부족합니다.");
                    }
                }
                else // 숫자가 아닌 입력
                {
                    Console.WriteLine("숫자를 입력해주세요.");
                }

                Console.WriteLine("계속하려면 아무 키나 누르세요...");
                Console.ReadKey();
            }
        }

        // ===== 던전 입장 함수 (미구현 안내) =====
        static void GoToDungeon()
        {
            Console.Clear(); // 화면 초기화
            Console.WriteLine("[던전 입장] 기능은 추후 업데이트 예정입니다.");
            Console.WriteLine("엔터를 누르면 돌아갑니다.");
            Console.ReadLine();
        }

        // ===== 아이템 가격 반환 함수 =====
        static int GetPrice(Item item)
        {
            return item.Name switch // 아이템 이름에 따라 가격 설정
            {
            "수련자 갑옷" => 1000,
            "무쇠갑옷" => 0,
            "스파르타의 갑옷" => 3500,
            "낡은 검" => 600,
            "청동 도끼" => 1500,
            "스파르타의 창" => 0,
                _ => 9999 // 기본값 (존재하지 않는 아이템)
            };
        }
    }
}
