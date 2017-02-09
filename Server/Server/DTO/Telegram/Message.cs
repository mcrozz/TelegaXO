using System;

namespace Server.DTO.Telegram
{
    public class Message
    {
        public Int32 message_id;
        public User from;
        public Int64 date;
        public String text;
        public Chat chat;
    }
}
