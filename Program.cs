using System;
using System.Collections.Generic;

namespace DataBase
{
    class Program
    {
        static void Main(string[] args)
        {
            Database database = new Database();
            database.Work();
        }
    }

    class Player
    {        
        private string _niсkname;
        private int _level;
        private bool _isBaned;

        public Player(int id, string username, int level, bool isBaned)
        {
            Id = id;
            _niсkname = username;
            _level = level;
            _isBaned = isBaned;
        }

        public int Id { get; private set; }

        public void Ban()
        {
            _isBaned = true;
        }

        public void UnBan()
        {
            _isBaned = false;
        }

        public void ShowInfo()
        {
            string status = ConvertIsBannedToText(_isBaned);

            Console.WriteLine($"ID {Id}. {_niсkname}, {_level} уровень, статус: {status}");
        }

        private string ConvertIsBannedToText(bool isBaned)
        {
            string convertedIsBannedToText = "активен";

            if (isBaned)
                convertedIsBannedToText = "забанен";

            return convertedIsBannedToText;
        }
    }

    class Database
    {
        private int _lastPlayerId;
        private List<Player> _players;

        public Database()
        {
            _lastPlayerId = 0;
            _players = new List<Player>();
        }

        public void Work()
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
            int id = ++_lastPlayerId;

            Console.WriteLine($"введите имя игрока:");
            string userName = Console.ReadLine();

            Console.WriteLine($"Введите уровень игрока");
            int.TryParse(Console.ReadLine(), out int level);

            if (level < 1)
            {
                level = 1;
                ShowSystemMessage($"Ошибка ввода данных.");
                Console.WriteLine($"Игроку присвоен {level}й уровень.");
            }

            _players.Add(new Player(id, userName, level, false));

            ShowSystemMessage("Запись успешно добавлена");
        }

        private void BanPlayer()
        {
            if (TryGetPlayer("Введите порядковый номер игрока для блокировки:", out Player player))
            {
                player.Ban();

                ShowSystemMessage("Игрок успешно заблокирован");
                player.ShowInfo();
            }
            else
            {
                ShowSystemMessage("Ошибка блокировки игрока");
            }
        }

        private void UnBanPlayer()
        {
            if (TryGetPlayer("Введите порядковый номер игрока для разблокировки:", out Player player))
            {
                player.UnBan();

                ShowSystemMessage("Игрок успешно разблокирован");
                player.ShowInfo();
            }
            else
            {
                ShowSystemMessage("Ошибка разблокировки игрока");
            }
        }

        private void DeletePlayer()
        {
            if (TryGetPlayer("Введите номер игрока для удаления:", out Player player))
            {
                _players.Remove(player);

                ShowSystemMessage("Игрок успешно удален");
                player.ShowInfo();
            }
            else
            {
                ShowSystemMessage("Ошибка удаления игрока");
            }
        }

        private bool TryGetPlayer(string message, out Player player)
        {
            player = null;
            bool isFindPlayer = false;

            Console.WriteLine(message);
            string numberByText = Console.ReadLine();

            int.TryParse(numberByText, out int number);

            for (int i = 0; i < _players.Count; i++)
            {
                if (_players[i].Id == number)
                {
                    player = _players[i];
                    isFindPlayer = true;
                }
            }

            return isFindPlayer;
        }

        private void DrawFrame(string text)
        {
            char horizontalSymbol = '-';
            char verticalSymbol = '|';

            string frame = "";
            string frameMessage = $"{verticalSymbol} {text} {verticalSymbol}";

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
