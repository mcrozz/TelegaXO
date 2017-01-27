
namespace Server
{
    class Room
    {
        public enum Fill : byte { empty, tic, tac };

        Room(Player player1, Player player2)
        {
            this->player1 = player1;
            this->player2 = player2;
            Players_Turn = 1;
            count1 = 0;
            count2 = 0;

            Game_Board = new Fill { {Fill.empty, Fill.empty, Fill.empty },
                                    {Fill.empty, Fill.empty, Fill.empty },
                                    {Fill.empty, Fill.empty, Fill.empty } };
        }

        public void UpdateState(Int32 ID, Int8 X, Int8 Y)
        {
            if (ID == player1.id && Players_Turn == 1 && X < 3 && Y < 3 && X >= 0 && Y >= 0 && Game_Board[X, Y] == Fill.empty)
            {
                Game_Board[X, Y] = Fill.tic;
                Players_Turn *= -1;
                player2.SendMessage(Game_Board);
                break;
            }

            if (ID == player2.id && Players_Turn == -1 && X < 3 && Y < 3 && X >= 0 && Y >= 0 && Game_Board[X, Y] == Fill.empty)
            {
                Game_Board[X, Y] = Fill.tac;
                Players_Turn *= -1;
                player1.SendMessage(Game_Board);
                break;
            }

            if (ID == player1.id && Players_Turn == -1)
            {
                count1++;
            }

            if (ID == player2.id && Players_Turn == 1)
            {
                count2++;
            }

            if ((count1 %10) == 0)
            {
                player1.SendMessage("Now is not your turn!");
            }

            if ((count2 % 10) == 0)
            {
                player2.SendMessage("Now is not your turn!");
            }
        }

        private Player player1;
        private Player player2;
        private int count1;
        private int count2;
        private Fill[,] Game_Board;
        private byte Players_Turn;
        public byte check_EndOfGame (Fill[,] Game_Board)
        {
            if ((Game_Board[0, 0] == Fill.tic && Game_Board[0, 1] == Fill.tic && Game_Board[0, 2] == Fill.tic) || (Game_Board[1, 0] == Fill.tic && Game_Board[1, 1] == Fill.tic && Game_Board[1, 2] == Fill.tic) || (Game_Board[2, 0] == Fill.tic && Game_Board[2, 1] == Fill.tic && Game_Board[2, 2] == Fill.tic) || (Game_Board[0, 0] == Fill.tic && Game_Board[1, 0] == Fill.tic && Game_Board[2, 0] == Fill.tic) || (Game_Board[0, 1] == Fill.tic && Game_Board[1, 1] == Fill.tic && Game_Board[2, 1] == Fill.tic) || (Game_Board[0, 2] == Fill.tic && Game_Board[1, 2] == Fill.tic && Game_Board[2, 2] == Fill.tic) || (Game_Board[0, 0] == Fill.tic && Game_Board[1, 1] == Fill.tic && Game_Board[2, 2] == Fill.tic) || (Game_Board[0, 2] == Fill.tic && Game_Board[1, 1] == Fill.tic && Game_Board[2, 0] == Fill.tic))
            {
                return 1; // player 1 win
            }

            if ((Game_Board[0, 0] == Fill.tac && Game_Board[0, 1] == Fill.tac && Game_Board[0, 2] == Fill.tac) || (Game_Board[1, 0] == Fill.tac && Game_Board[1, 1] == Fill.tac && Game_Board[1, 2] == Fill.tac) || (Game_Board[2, 0] == Fill.tac && Game_Board[2, 1] == Fill.tac && Game_Board[2, 2] == Fill.tac) || (Game_Board[0, 0] == Fill.tac && Game_Board[1, 0] == Fill.tac && Game_Board[2, 0] == Fill.tac) || (Game_Board[0, 1] == Fill.tac && Game_Board[1, 1] == Fill.tac && Game_Board[2, 1] == Fill.tac) || (Game_Board[0, 2] == Fill.tac && Game_Board[1, 2] == Fill.tac && Game_Board[2, 2] == Fill.tac) || (Game_Board[0, 0] == Fill.tac && Game_Board[1, 1] == Fill.tac && Game_Board[2, 2] == Fill.tac) || (Game_Board[0, 2] == Fill.tac && Game_Board[1, 1] == Fill.tac && Game_Board[2, 0] == Fill.tac))
            {
                return 2; // player 2 win
            }

            if (Game_Board[0, 0] != Fill.empty && Game_Board[0, 1] != Fill.empty && Game_Board[0, 2] != Fill.empty && Game_Board[1, 0] != Fill.empty && Game_Board[1, 1] != Fill.empty && Game_Board[1, 2] != Fill.empty && Game_Board[2, 0] != Fill.empty && Game_Board[2, 1] != Fill.empty && Game_Board[2, 2] != Fill.empty)
            {
                return 3; // tie
            }

            else
            {
                return 0; // nothing interesting, next turn
            }
        }

        
       
    }
}
