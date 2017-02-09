using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;

namespace Server
{
    public static class Game
    {
        public static void Start()
        {
            Telegram.Start(ref _telegram);
            _gameThread = new Thread(Game.GameLoop);
            _gameThread.Start();
        }

        public static void Queue(DTO.Telegram.Message message)
        {
            _queueMessages.Add(message);
        }

        public static void Queue(DTO.Telegram.CallbackQuery message)
        {
            _queueCallback.Add(message);
        }

        private static void GameLoop()
        {
            while (true)
            {
                while (_queueMessages.Count != 0)
                {
                    var message = _queueMessages[0];
                    _queueMessages.RemoveAt(0);
                    ParseNormalMessage(message);
                }

                while (_queueCallback.Count != 0)
                {
                    var message = _queueCallback[0];
                    _queueMessages.RemoveAt(0);
                    ParseQueryUpdate(message);
                }

                Thread.Sleep(10);
            }
        }

        private static void StartGame(DTO.Telegram.User player1, DTO.Telegram.User player2 = null)
        {
            IPlayer _player1 = new Player(player1);
            IPlayer _player2 = new Player(player2);
            Room room = new Room(_player1, _player2);
            _rooms.Add(room);
        }

        private static void ParseNormalMessage(DTO.Telegram.Message message)
        {
            Console.WriteLine($"[{message.@from.username}]: {message.text}");

            if (message.text.StartsWith("/"))
            {
                String command = message.text.Substring(1);
                command = command.Trim();
                switch (command)
                {
                    case "start":
                        StartGame(message.@from);
                        break;
                    default:
                        break;
                }
            }

            // For testing purposes only
            // <<
            // TODO: remove
            if (message.text.Equals("Hey"))
            {
                DTO.Message reply = new DTO.Message
                {
                    User = message.@from,
                    Text = "Good day"
                };
                Telegram.Send(reply);
            }/*
                else if (message.message.text.Equals("Test"))
                {
                    DTO.Telegram.KeyboardButton[,] buttons = new DTO.Telegram.KeyboardButton[2, 2]
                    {
                        {new DTO.Telegram.KeyboardButton("Hey"), new DTO.Telegram.KeyboardButton("Don't") },
                        {new DTO.Telegram.KeyboardButton("Stop"), new DTO.Telegram.KeyboardButton("Test") }
                    };
                    DTO.Telegram.ReplyKeyboardMarkup keyboard = new DTO.Telegram.ReplyKeyboardMarkup
                    {
                        keyboard = buttons,
                        one_time_keyboard = true,
                        resize_keyboard = true
                    };
                    DTO.Message reply = new DTO.Message
                    {
                        User = message.message.@from,
                        Text = "Choose wisely",
                        KeyboardMarkup = keyboard
                    };
                    Telegram.Send(reply);
                }*/
            else if (message.text.Equals("Test"))
            {
                Int32 size = 8;
                String[] XY = new String[] {"X", "O", " "};
                Random random = new Random(new TimeSpan().Milliseconds);
                Func<String> XorY = new Func<String>(() => XY[random.Next(0, 3)]);
                DTO.Telegram.InlineKeyboardButton[,] buttons = new DTO.Telegram.InlineKeyboardButton[size, size];
                for (Int32 i=0; i<size; i++)
                    for (Int32 j = 0; j < size; j++)
                        buttons[i, j] = new DTO.Telegram.InlineKeyboardButton(XorY(), $"{i}_{j}");

                DTO.Telegram.InlineKeyboardMarkup keyboard = new DTO.Telegram.InlineKeyboardMarkup()
                {
                    inline_keyboard = buttons
                };
                DTO.Message reply = new DTO.Message()
                {
                    User = message.@from,
                    Text = "Select one",
                    KeyboardMarkup = keyboard
                };
                Telegram.Send(reply);
            }
            // >>
        }

        private static void ParseQueryUpdate(DTO.Telegram.CallbackQuery query)
        {
            Console.WriteLine($"Got callbackQuery with data: {query.data}");

            Boolean foundRoom = false;
            foreach (Room room in _rooms)
            {
                if (room.isPlayerPresent(query.@from.id))
                {
                    Match point = extractData.Match(query.data);
                    if (!point.Success)
                        throw new Exception(ExcDataNotExctracted);
                    room.UpdateState(query.@from.id, (Int16)point.Groups[0], (Int16)point.Groups[1]);
                    foundRoom = true;
                }
            }
            if (foundRoom)
                return;

            DTO.Telegram.AnswerCallbackQuery answerQuery = new DTO.Telegram.AnswerCallbackQuery
            {
                callback_query_id = query.id,
                text = ExcRoomNotFound
            };
            DTO.Message reply = new DTO.Message
            {
                User = query.@from,
                AnswerCallbackQuery = answerQuery
            };
            Telegram.Send(reply);
        }

        private static IList<Room> _rooms = new List<Room>();
        private static Telegram _telegram = new Telegram();

        private static IList<DTO.Telegram.Message> _queueMessages = new List<DTO.Telegram.Message>();
        private static IList<DTO.Telegram.CallbackQuery> _queueCallback = new List<DTO.Telegram.CallbackQuery>();

        private static Thread _gameThread;

        private static readonly Regex extractData = new Regex(@"^(\d)_(\d)$");

        private const String ExcRoomNotFound = "Maybe you wanted to begin game?";
        private const String ExcDataNotExctracted = "Could not extract data from callbackQuery";
    }
}
