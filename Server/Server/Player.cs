// Newton library
namespace Server
{
    abstract class IPlayer
    {
        public IPlayer(DTO.Telegram.User player)
        {
            this.id = player.id;
        }
        public abstract void SendMessage(string text);
        //public abstract void SendMessage(IDictionary<Int8, Int8> obj);
        public System.Int32 getID()
        {
            return this.id;
        }

        //private DTO.Telegram.User Personality
        private System.Int32 id;
    };

    class Player : IPlayer
    {
        public override void SendMessage(string text)
        {
            DTO.Message reply = new DTO.Message();
            reply.Text = text;
            Telegram.Send(reply);
        }
    }
}