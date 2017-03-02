using System;
using System.Web.Hosting;
using System.Web.UI.WebControls;
using Server.DTO.Telegram;


namespace Server
{
    class AI_Player : IPlayer
    {

        public AI_Player(Room.Fill Marker)
        {
            this.user.id = -1;
            this.user.username = "bot";
            this.marker = Marker;
        }

        public Int32 ID
        {
            get { return user.id; }
        }

        public void SendMessage(Room.Fill[,] board)
        {
            /*if (in one row/Column are two enemy markers)
                IApplicationPreloadUtil own marker between them */
            if (board[1, 1] == Room.Fill.empty)
                board[1, 1] = marker;
            // smth like "SendMessage", isn't it ?
            else if (true)
                return;
            

            DTO.Telegram.CallbackQuery BotReply = new DTO.Telegram.CallbackQuery
            {
                id = String.Empty,
                from = this.user,
                data = String.Concat('_')
            };

            Game.Queue(BotReply);
        }

        public void SendMessage(string text)
        {
            // Mock
        }

        private readonly DTO.Telegram.User user;
        private readonly Room.Fill marker;
    }
}
