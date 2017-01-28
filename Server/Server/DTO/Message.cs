using System;

namespace Server.DTO
{
    public class Message
    {
        public Telegram.User User;
        public String Text;
        public Telegram.InlineKeyboardMarkup KeyboardMarkup;
    }
}
