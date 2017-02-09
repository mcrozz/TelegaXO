using System;

namespace Server
{
    interface IPlayer
    {
        Int32 ID { get; }
        void SendMessage(Room.Fill[,] cell);
        void SendMessage(string text);
    }
}