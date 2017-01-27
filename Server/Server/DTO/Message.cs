using System;

namespace Server.DTO
{
    class Message
    {
        public Message() { }

        public Telegram.User User
        {
            get { return user; }
            set { user = value; }
        }

        private Telegram.User user;

        public String Text
        {
            get { return text; }
            set { text = value; }
        }

        private String text;
    }
}
