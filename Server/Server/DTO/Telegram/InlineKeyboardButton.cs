using System;

namespace Server.DTO.Telegram
{
    public class InlineKeyboardButton
    {
        public InlineKeyboardButton() { }

        public InlineKeyboardButton(String text, String callback)
        {
            this.text = text;
            this.callback_data = callback;
        }

        public String text;
        public String callback_data;
    }
}
