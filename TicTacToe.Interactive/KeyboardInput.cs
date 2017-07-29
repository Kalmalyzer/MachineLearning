using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Simulation;

namespace TicTacToe.Interactive
{
    public class KeyboardInput
    {
        public static PlayerInput GetPlayerInput(GameState gameState)
        {
            int x = -1;
            int y = -1;

            while (true)
            {
                while (true)
                {
                    Console.Write("X: ");
                    if (Int32.TryParse(Console.ReadLine(), out x))
                        if (x >= 0 && x < BoardState.Width)
                            break;
                }
                while (true)
                {
                    Console.Write("Y: ");
                    if (Int32.TryParse(Console.ReadLine(), out y))
                        if (y >= 0 && y < BoardState.Height)
                            break;
                }

                if (gameState.BoardState.Positions[x, y] == BoardState.Player.None)
                    break;
            }

            Console.WriteLine();

            return new PlayerInput(gameState.NextPlayer, x, y);
        }
    }
}
