using System;

namespace Server.DTO.Telegram
{
    public class KeyboardButton
    {
        public KeyboardButton() { }

        public KeyboardButton(String text)
        {
            this.text = text;
        }

        public String text;
    }
}
