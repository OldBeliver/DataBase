﻿using System;
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
            Database dataBase = new Database();
            dataBase.Work();
        }
    }

    class Player
    {
        private int _id;
        private string _niсkname;
        private int _level;
        private bool _isBaned;

        public Player(int id, string username, int level, bool isBaned)
        {
            _id = id;
            _niсkname = username;
            _level = level;
            _isBaned = isBaned;
        }

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
            string status = ConvertBoolStatusToText(_isBaned);

            Console.WriteLine($"ID {_id}. {_niсkname}, {_level} уровень, статус: {status}");
        }

        private string ConvertBoolStatusToText(bool isBaned)
        {
            string statusInfo = "аткивен";

            if (isBaned)
                statusInfo = "забанен";

            return statusInfo;
        }
    }

    class Database
    {
        private int _playerId;
        private List<Player> _players;

        public Database()
        {
            _playerId = 1;
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
            int id = CreatePlayerId();

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
            }
            else
            {
                ShowSystemMessage("Ошибка разблокировки игрока");
            }
        }

        private void DeletePlayer()
        {            
            if(TryGetPlayer("Введите номер игрока для удаления:", out Player player))
            {
                _players.Remove(player);

                ShowSystemMessage("Игрок успешно удален");
            }
            else
            {
                ShowSystemMessage("Ошибка удаления игрока");
            }
        }

        private int CreatePlayerId()
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

            return _playerId++;
        }

        private bool TryGetPlayer(string message, out Player player)
        {
            player = null;
            int index = 0;
            bool isFindPlayer = false;            
            
            Console.WriteLine(message);
            string numberByText = Console.ReadLine();

            int.TryParse(numberByText, out int number);

            if (0 < number && number <= _players.Count)
            {
                index = number - 1;
                player = _players[index];
                isFindPlayer = true;
            }

            return isFindPlayer;
        }

        private bool TryGetNumber(string message, out int number)
        {
            Console.WriteLine(message);
            string numberByText = Console.ReadLine();

            return int.TryParse(numberByText, out number);
        }

        private bool TryGetIndex(int number, out int index)
        {
            index = 0;

            bool isCorrectRange = false;

            if (0 < number && number <= _players.Count)
            {
                index = number - 1;

                isCorrectRange = true;
            }

            return isCorrectRange;
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
