using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    class Program
    {
        static void Main(string[] args)
        {
            DataBase dataBase = new DataBase();
            dataBase.ShowMenu();
        }
    }

    class Player
    {
        private int _id;
        private string _niсkname;
        private int _level;
        private bool _isBan;

        public Player(int id, string username, int level, bool isBan)
        {
            _id = id;
            _niсkname = username;
            _level = level;
            _isBan = isBan;
        }        

        public void Ban()
        {
            _isBan = true;
        }

        public void UnBan()
        {
            _isBan = false;
        }

        public void ShowInfo()
        {
            string status = ShowStatus(_isBan);

            Console.WriteLine($"ID {_id}. {_niсkname}, {_level} уровень, статус: {status}");
        }

        private string ShowStatus(bool status)
        {
            string statusInfo = "аткивен";

            if (status)
                statusInfo = "забанен";

            return statusInfo;
        }
    }

    class DataBase
    {
        private int _id;
        private List<Player> _players;

        public DataBase()
        {
            _id = 1;
            _players = new List<Player>();
        }

        public void ShowMenu()
        {
            const string CommandShowPlayers = "1";
            const string CommandAddPlayer = "2";
            const string CommandBanPlayer = "3";
            const string CommandUnBanPlayer = "4";
            const string CommandDeletePlayer = "5";
            const string CommandExit = "6";

            bool isWork = true;
            string title = "База данных игроков";

            while (isWork)
            {
                DrawFrame(title);

                Console.WriteLine($"{CommandShowPlayers}. Показать всех игроков");
                Console.WriteLine($"{CommandAddPlayer}. Добавить игрока");
                Console.WriteLine($"{CommandBanPlayer}. Забанить игрока");
                Console.WriteLine($"{CommandUnBanPlayer}. Разбанить игрока");
                Console.WriteLine($"{CommandDeletePlayer}. Удалить игрока");
                Console.WriteLine($"{CommandExit}. Выход");

                Console.WriteLine($"\nВведите команду:");
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case CommandShowPlayers:
                        ShowPlayers();
                        break;
                    case CommandAddPlayer:
                        AddPlayer();
                        break;
                    case CommandBanPlayer:
                        BanPlayer();
                        break;
                    case CommandUnBanPlayer:
                        UnBanPlayer();
                        break;
                    case CommandDeletePlayer:
                        DeletePlayer();
                        break;
                    case CommandExit:
                        isWork = false;
                        break;
                    default:
                        ShowSystemMessage($"Ошибка ввода команды");
                        break;
                }
            }
        }

        private void ShowPlayers()
        {
            if (_players.Count == 0)
            {
                ShowSystemMessage($"База пустая");
            }

            foreach (Player player in _players)
            {
                player.ShowInfo();
            }
        }

        private void AddPlayer()
        {
            Console.WriteLine($"введите имя игрока:");
            string userName = Console.ReadLine();            

            if (TryGetNumber($"введите уровень игрока:", out int level) == false)
            {
                level = 1;

                ShowSystemMessage($"Ошибка ввода данных.");
                Console.WriteLine($"Игроку присвоен {level}й уровень.");
            }

            int id = CreateId();

            Player newPlayer = new Player(id, userName, level, false);

            AddRecord(newPlayer);
        }

        private void AddRecord(Player player)
        {
            _players.Add(player);

            ShowSystemMessage("Запись успешно добавлена");
        }

        private void BanPlayer()
        {
            if(TryGetNumber("Введите номер для блокировки игрока", out int positionNumber)== false)
            {
                ShowSystemMessage($"Ошибка ввода данных");
                return;
            }

            if (TryGetIndex(positionNumber, _players, out int index) == false)
            {
                ShowSystemMessage($"Ошибка ввода данных");
                return;
            }

            _players[index].Ban();
        }

        private void UnBanPlayer()
        {
            if(TryGetNumber("Введите номер для разблокировки игрока", out int positionNumber) == false)
            {
                ShowSystemMessage($"Ошибка ввода данных");
                return;
            }

            if(TryGetIndex(positionNumber, _players, out int index)== false)
            {
                ShowSystemMessage($"Ошибка ввода данных");
                return;
            }

            _players[index].UnBan();
        }

        private void DeletePlayer()
        {
            if(TryGetNumber("Введите номер игрока для удаления:", out int positionNumber) == false)
            {
                ShowSystemMessage($"Ошибка ввода данных");
                return;
            }

            if(TryGetIndex(positionNumber, _players, out int index) == false)
            {
                ShowSystemMessage($"Ошибка ввода данных");
                return;
            }

            _players.RemoveAt(index);
        }

        private int CreateId()
        {
            // 1й вариант получения ID
            // return _id++;

            // 2й вариант получения ID через GetHashCode
            // private int CreateId(string userName, int level)
            // string textForId = $"{userName}{level}";
            // id = textForId.GetHashCode();

            // 3й вариант получения ID через Guid
            // у _id должен быть тип переменной string 
            // Guid guid = Guid.NewGuid();
            // string textId = guid.ToString();

            return _id++;
        }

        private bool TryGetNumber(string message, out int number)
        {
            Console.WriteLine(message);
            string numberByText = Console.ReadLine();

            return int.TryParse(numberByText, out number);
        }

        private bool TryGetIndex(int number, List<Player> players, out int index)
        {
            index = 0;

            bool isCorrectRange = false;

            if (0 < number && number <= players.Count)
            {
                index = number - 1;

                isCorrectRange = true;
            }

            return isCorrectRange;
        }

        private void DrawFrame(string text)
        {
            char horizontalSymbol = '-';
            char VerticalSymbol = '|';

            string frame = "";
            string frameMessage = $"{VerticalSymbol} {text} {VerticalSymbol}";

            for (int i = 0; i < frameMessage.Length; i++)
            {
                frame += horizontalSymbol;
            }

            Console.WriteLine(frame);
            Console.WriteLine(frameMessage);
            Console.WriteLine(frame);
        }

        private void ShowSystemMessage(string message, ConsoleColor color = ConsoleColor.Red)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
