using System;

namespace Server.DTO.Telegram
{
    public class Update
    {
        public Int32 update_id;
        public Message message;
        public CallbackQuery callback_query;
    }
}
