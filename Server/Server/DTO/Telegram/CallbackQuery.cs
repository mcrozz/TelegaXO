using System;

namespace Server.DTO.Telegram
{
    public class CallbackQuery
    {
        public String id;
        public User from;
        public String data;
    }
}
