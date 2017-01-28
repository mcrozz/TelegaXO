using System;
using System.Collections.Generic;

namespace Server
{
    public static class Game
    {

        public static void Start()
        {
            Telegram.Start(ref telegram);
        }

        // TODO: implement for PvP
        public static void StartGame(DTO.Telegram.User player1, DTO.Telegram.User player2)
        {
        }

        public static void StartGame(DTO.Telegram.User player)
        {
        }

        public static void ParseNormalMessage(DTO.Telegram.Message message)
        {
            Console.WriteLine($"[{message.@from.username}]: {message.text}");

            if (message.text.StartsWith("/"))
            {
                String command = message.text.Substring(1);
                command = command.Trim();
                switch (command)
                {
                    case "start":
                        // TODO: StartGame
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

        public static void ParseQueryUpdate(DTO.Telegram.CallbackQuery query)
        {
            Console.WriteLine($"Got callbackQuery with data: {query.data}");

            // TODO: forward it to the Room[].UpdateState
            DTO.Telegram.AnswerCallbackQuery answerQuery = new DTO.Telegram.AnswerCallbackQuery
            {
                callback_query_id = query.id,
                text = "Your anser accepted"
            };
            DTO.Message reply = new DTO.Message
            {
                User = query.@from,
                AnswerCallbackQuery = answerQuery
            };
            Telegram.Send(reply);
        }

        private static IList<Room> rooms = new List<Room>();
        private static Telegram telegram = new Telegram();
    }
}
