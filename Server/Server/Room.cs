
namespace Server
{
    class Room
    {
        public enum Fill : byte { empty, tic, tac };
        public enum EndOfGame : byte { player1, player2, tie};

        public Room(Player player1, Player player2)
        {
            this.player1 = player1;
            this.player2 = player2;
            Players_Turn = 1;
            Game_Board = new Fill { {Fill.empty, Fill.empty, Fill.empty },
                                    {Fill.empty, Fill.empty, Fill.empty },
                                    {Fill.empty, Fill.empty, Fill.empty } };
        }

        public void UpdateState(Int32 ID, Int8 X, Int8 Y)
        {
            if (ID == player1.id && Players_Turn == -1)
            {
                 player1.SendMessage("Now is not your turn!");
                 return;
            }

            if (ID == player2.id && Players_Turn == 1)
            {
                player2.SendMessage("Now is not your turn!");
                return;
            }

   
            if (ID == player1.id && Game_Board[X, Y] == Fill.empty)
            {
                Game_Board[X, Y] = Fill.tic;
                Players_Turn *= -1;
                player2.SendMessage(Game_Board);

                if (check_EndOfGame(Game_Board) == EndOfGame.player1)
                {
                    player1.SendMessage("You win!");
                    player2.SendMessage("You lose!");
                }
                if (check_EndOfGame(Game_Board) == EndOfGame.player2)
                {
                    player2.SendMessage("You win!");
                    player1.SendMessage("You lose!");
                }
                if (check_EndOfGame(Game_Board) == EndOfGame.tie)
                {
                    player1.SendMessage("Tie!");
                    player2.SendMessage("Tie!");
                }
                return;
            }

            if (ID == player2.id && Game_Board[X, Y] == Fill.empty)
            {
                Game_Board[X, Y] = Fill.tac;
                Players_Turn *= -1;
                player1.SendMessage(Game_Board);

                if (check_EndOfGame(Game_Board) == EndOfGame.player1)
                {
                    player1.SendMessage("You win!");
                    player2.SendMessage("You lose!");
                }
                if (check_EndOfGame(Game_Board) == EndOfGame.player2)
                {
                    player2.SendMessage("You win!");
                    player1.SendMessage("You lose!");
                }
                if (check_EndOfGame(Game_Board) == EndOfGame.tie)
                {
                    player1.SendMessage("Tie!");
                    player2.SendMessage("Tie!");
                }
                return;
            }
        }

        private Player player1;
        private Player player2;
        private Fill[,] Game_Board;
        private byte Players_Turn;
        public byte check_EndOfGame(Fill[,] Game_Board)
        {
            bool Tie = true;
            int Size = System.Convert.ToInt32(System.Math.Sqrt(Game_Board.Length));
            for (int i = 0; i < Size - 2; i++)
            {
                for (int j = 0; j < Size - 2; j++)
                {

                    // Проверка на 3 подряд по вертикали и по горизонтали кроме четырех случаев в правом нижнем углу
                    if (Game_Board[i, j] == Fill.tic && Game_Board[i, j + 1] == Fill.tic && Game_Board[i, j + 2] == Fill.tic)
                    {
                        return EndOfGame.player1;
                    }
                    if (Game_Board[i, j] == Fill.tac && Game_Board[i, j + 1] == Fill.tac && Game_Board[i, j + 2] == Fill.tac)
                    {
                        return EndOfGame.player2;
                    }
                    if (Game_Board[i, j] == Fill.tic && Game_Board[i + 1, j] == Fill.tic && Game_Board[i + 2, j] == Fill.tic)
                    {
                        return EndOfGame.player1;
                    }
                    if (Game_Board[i, j] == Fill.tac && Game_Board[i + 1, j] == Fill.tac && Game_Board[i + 2, j] == Fill.tac)
                    {
                        return EndOfGame.player2;
                    }
                    // Проверка по диагонали, сонаправленной с главной
                    if (Game_Board[i, j] == Fill.tic && Game_Board[i + 1, j + 1] == Fill.tic && Game_Board[i + 2, j + 2] == Fill.tic)
                    {
                        return EndOfGame.player1;
                    }
                    if (Game_Board[i, j] == Fill.tac && Game_Board[i + 1, j + 1] == Fill.tac && Game_Board[i + 2, j + 2] == Fill.tac)
                    {
                        return EndOfGame.player2;
                    }
                    // Проверка по второй диагонали
                    if (Game_Board[i + 2, j] == Fill.tic && Game_Board[i + 1, j + 1] == Fill.tic && Game_Board[i, j + 2] == Fill.tic)
                    {
                        return EndOfGame.player1;
                    }
                    if (Game_Board[i + 2, j] == Fill.tac && Game_Board[i + 1, j + 1] == Fill.tac && Game_Board[i, j + 2] == Fill.tac)
                    {
                        return EndOfGame.player2;
                    }
                }
            }
            // Оставшиеся 4 случая, игрок 1
            if (Game_Board[Size, Size] == Fill.tic && Game_Board[Size, Size - 1] == Fill.tic && Game_Board[Size, Size - 2] == Fill.tic)
            {
                return EndOfGame.player1;
            }
            if (Game_Board[Size - 1, Size] == Fill.tic && Game_Board[Size - 1, Size - 1] == Fill.tic && Game_Board[Size - 1, Size - 2] == Fill.tic)
            {
                return EndOfGame.player1;
            }
            if (Game_Board[Size, Size] == Fill.tic && Game_Board[Size - 1, Size] == Fill.tic && Game_Board[Size - 2, Size] == Fill.tic)
            {
                return EndOfGame.player1;
            }
            if (Game_Board[Size, Size - 1] == Fill.tic && Game_Board[Size - 1, Size - 1] == Fill.tic && Game_Board[Size - 2, Size - 1] == Fill.tic)
            {
                return EndOfGame.player1;
            }
            // они же, игрок 2
            if (Game_Board[Size, Size] == Fill.tac && Game_Board[Size, Size - 1] == Fill.tac && Game_Board[Size, Size - 2] == Fill.tac)
            {
                return EndOfGame.player2;
            }
            if (Game_Board[Size - 1, Size] == Fill.tac && Game_Board[Size - 1, Size - 1] == Fill.tac && Game_Board[Size - 1, Size - 2] == Fill.tac)
            {
                return EndOfGame.player2;
            }
            if (Game_Board[Size, Size] == Fill.tac && Game_Board[Size - 1, Size] == Fill.tac && Game_Board[Size - 2, Size] == Fill.tac)
            {
                return EndOfGame.player2;
            }
            if (Game_Board[Size, Size - 1] == Fill.tac && Game_Board[Size - 1, Size - 1] == Fill.tac && Game_Board[Size - 2, Size - 1] == Fill.tac)
            {
                return EndOfGame.player2;
            }
            // Проверка на патовую ситуацию
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (Game_Board[i, j] == Fill.empty)
                    {
                        Tie = false;
                        break;
                    }
                }
            }
            if (Tie == true)
            {
                return EndOfGame.tie;
            }
        }
               

        public Boolean isPlayerPresent (Int32 ID)
        {
            if (player1.id == ID || player2.id == ID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
       
    }
}
