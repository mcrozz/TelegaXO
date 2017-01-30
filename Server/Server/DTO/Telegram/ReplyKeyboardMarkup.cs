using System;

namespace Server.DTO.Telegram
{
    public class ReplyKeyboardMarkup
    {
        public KeyboardButton[,] keyboard;
        public Boolean resize_keyboard;
        public Boolean one_time_keyboard;
    }
}
