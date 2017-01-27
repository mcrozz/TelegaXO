using System.Threading;
using System.Collections.Generic;

namespace Server
{
    public class Game
    {
        public Game()
        {
            rooms = new List<Room>();
            telegram = new Telegram();
            Telegram.Start(ref telegram);
        }

        public void StartGame()
        {
        }

        private IList<Room> rooms;
        private Telegram telegram;
    }
}
