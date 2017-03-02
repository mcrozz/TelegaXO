using System;

namespace Server
{
    class Player : IPlayer
    {
        [Obsolete("Igor' ne delay etogo!")]
        public Player(DTO.Telegram.User player)
        {
            this.user = player;
        }


        public Player(DTO.Telegram.User player, Room.Fill Marker)
        {
            this.user = player;
            this.marker = Marker;
        }

        public Int32 ID
        {
            get { return this.user.id; }
        }

        public void SendMessage(Room.Fill[,] cell)
        {
            Int32 size = cell.GetLength(0);
            DTO.Telegram.InlineKeyboardButton[,] buttons = new DTO.Telegram.InlineKeyboardButton[size, size];
            for (Int32 i = 0; i < size; i++)
            {
                for (Int32 j = 0; j < size; j++)
                {
                    switch (cell[i, j])
                    {
                        case Room.Fill.empty:
                            buttons[i, j] = new DTO.Telegram.InlineKeyboardButton(" ", $"{i}_{j}");
                            break;
                        case Room.Fill.tic:
                            buttons[i, j] = new DTO.Telegram.InlineKeyboardButton("X", $"{i}_{j}");
                            break;
                        case Room.Fill.tac:
                            buttons[i, j] = new DTO.Telegram.InlineKeyboardButton("O", $"{i}_{j}");
                            break;
                        default:
                            break;
                    }
                }
            }

            DTO.Telegram.InlineKeyboardMarkup keyboard = new DTO.Telegram.InlineKeyboardMarkup
            {
                inline_keyboard = buttons
            };

            DTO.Message reply = new DTO.Message
            {
                User = this.user,
                KeyboardMarkup = keyboard
            };
            Telegram.Send(reply);
        }

        public void SendMessage(string text)
        {
            DTO.Message reply = new DTO.Message
            {
                User = this.user,
                Text = text
            };
            Telegram.Send(reply);
        }


        private readonly DTO.Telegram.User user;
        private readonly Room.Fill marker;
    }
}