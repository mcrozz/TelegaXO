using System;

namespace Server.DTO
{
    class MessageDTO
    {
        MessageDTO() { }

        public String User
        {
            get { return user; }
            set { user = value; }
        }

        private String user;

        public String Message
        {
            get { return message; }
            set { message = value; }
        }

        private String message;
    }
}
